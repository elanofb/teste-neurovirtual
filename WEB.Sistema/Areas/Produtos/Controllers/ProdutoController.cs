using System;
using System.Linq;
using System.Web.Mvc;
using BLL.Produtos;
using DAL.Produtos;
using PagedList;
using WEB.Areas.Produtos.ViewModels;
using System.Json;
using BLL.Arquivos;
using DAL.Entities;
using MvcFlashMessages;
using WEB.Helpers;
using WEB.Areas.Associacoes.ViewModels;
using WEB.Extensions;

namespace WEB.Areas.Produtos.Controllers {

    [OrganizacaoFilter]
    public class ProdutoController : Controller {

        //Constantes

        //Atributos
        private IProdutoBL _IProdutoBL;
        private IProdutoComposicaoConsultaBL _IProdutoComposicaoConsultaBL;
        private IProdutoComposicaoCadastroBL _IProdutoComposicaoCadastroBL;
        private IProdutoComposicaoExclusaoBL _IProdutoComposicaoExclusaoBL;
        private IArquivoUploadFotoBL _IArquivoUploadFotoBL;
        private IProdutoTipoAssociadoBL _ProdutoTipoAssociadoBL;
        private IProdutoSituacaoConsultaBL _ProdutoSituacaoBL;

        //Propriedades
        private IProdutoBL OProdutoBL => _IProdutoBL = _IProdutoBL ?? new ProdutoBL();
        private IProdutoComposicaoConsultaBL OProdutoComposicaoConsultaBL => _IProdutoComposicaoConsultaBL = _IProdutoComposicaoConsultaBL ?? new ProdutoComposicaoConsultaBL();
        private IProdutoComposicaoCadastroBL OProdutoComposicaoCadastroBL => _IProdutoComposicaoCadastroBL = _IProdutoComposicaoCadastroBL ?? new ProdutoComposicaoCadastroBL();
        private IProdutoComposicaoExclusaoBL OProdutoComposicaoExclusaoBL => _IProdutoComposicaoExclusaoBL = _IProdutoComposicaoExclusaoBL ?? new ProdutoComposicaoExclusaoBL();
        private IArquivoUploadFotoBL OArquivoUploadFotoBL => _IArquivoUploadFotoBL = _IArquivoUploadFotoBL ?? new ArquivoUploadFotoBL();
        private IProdutoTipoAssociadoBL OProdutoTipoAssociadoBL => _ProdutoTipoAssociadoBL = _ProdutoTipoAssociadoBL ?? new ProdutoTipoAssociadoBL();

        private IProdutoSituacaoConsultaBL OProdutoSituacaoBL => _ProdutoSituacaoBL = _ProdutoSituacaoBL ?? new ProdutoSituacaoConsultaBL();

        //
        public ProdutoController() {
        }

        //
        public ActionResult listar() {

            string descricao = UtilRequest.getString("valorBusca");

            bool? ativo = UtilRequest.getBool("flagAtivo");

            string flagProdServ = UtilRequest.getString("flagProdServ");

            int idTipoProduto = UtilRequest.getInt32("idTipoProduto");

            string flagTipoSaida = UtilRequest.getString("flagTipoSaida");

            var queryProdutos = this.OProdutoBL.listar(idTipoProduto, descricao, ativo, flagProdServ);

            if (flagTipoSaida == TipoSaidaHelper.EXCEL) {

                var OProdutoExportacao = new ProdutoExportacao();

                OProdutoExportacao.baixarExcel(queryProdutos.ToList());

                return null;
            }
            
            var listaProdutos = queryProdutos.Select(x => new {
                x.id,
                x.idTipoProduto,
                x.idFornecedor,
                x.nome,
                x.descricaoResumida,
                x.valor,
                x.idOrganizacao,
                x.ativo,
                x.flagOnline,
                x.dtCadastro,
                x.flagParaTodos,
                x.flagSomenteAssociados,
                x.flagSomenteAssociadosQuites,
                x.flagValorConfiguravel,
                x.flagCortesia,
                x.qtdeDiasDuracao,
                TipoProduto = new { id = x.idTipoProduto, descricao = x.TipoProduto.descricao, x.TipoProduto.flagServico, x.TipoProduto.flagProduto },
            }).OrderBy(x => x.nome).ToPagedListJsonObject<Produto>(UtilRequest.getNroPagina(), UtilRequest.getNroRegistros());

            var idsProdutos = listaProdutos.Select(x => x.id).ToArray();

            var listaFotos = this.OArquivoUploadFotoBL.listar(0, EntityTypes.PRODUTO, "S")
                                 .Where(x => idsProdutos.Contains(x.idReferenciaEntidade)).ToList();

            foreach (var OProduto in listaProdutos) {
                OProduto.listaFotos = listaFotos.Where(x => x.idReferenciaEntidade == OProduto.id).ToList();
            }

            return View(listaProdutos);
        }

        [ActionName("aba-composicao")]
        public ActionResult Composicao(int idProduto = 0) {

            var ViewModel = new ProdutoComposicaoVM();

            ViewModel.listaProdutoComposicao = OProdutoComposicaoConsultaBL.listar(true).Where(x => x.idProduto == idProduto).OrderByDescending(x => x.dtCadastro).ToPagedList(UtilRequest.getNroPagina(),UtilRequest.getNroRegistros());

            ViewModel.idProduto = idProduto;

            return View(ViewModel);
        }

        [ActionName("partial-form-adicionar-item")]
        public ActionResult FormAdicionar(int idProduto = 0) {

            var ViewModel = new ProdutoComposicaoForm();

            ViewModel.ProdutoComposicao.idProduto = idProduto;

            return View(ViewModel);
        }

        [ActionName("adiconar-item")]
        public ActionResult Adicionar(ProdutoComposicaoForm ViewModel) {

            if (!ModelState.IsValid) {
                return PartialView("partial-form-adicionar-item", ViewModel);
            }

            bool flagSucesso = OProdutoComposicaoCadastroBL.salvar(ViewModel.ProdutoComposicao);

            if (flagSucesso) {
                return Json(new { error = false, message = "Adicionado com sucesso" });
            }

            return Json(new { error = true, message = "Erro ao adicionar" });
        }

        //Carregamento de formulario para edicao ou inclusao de novo registro
        [HttpGet]
        public ActionResult editar(int? id) {

            var ViewModel = new ProdutoForm();

            ViewModel.Produto = this.OProdutoBL.carregar(UtilNumber.toInt32(id)) ?? new Produto();

            if (ViewModel.Produto.id == 0) {

                ViewModel.flagQuites = "todos";

            }

            if (ViewModel.Produto.id > 0) {

                ViewModel.listaIdsTipoAssociado = this.OProdutoTipoAssociadoBL.query(ViewModel.Produto.id).Select(x => x.idTipoAssociado).ToList();

                ViewModel.listaProdutoSituacao = this.OProdutoSituacaoBL.query().Select(x => x.descricao).ToList();

            }

            return View(ViewModel);
        }

        //
        [HttpPost]
        public ActionResult editar(ProdutoForm ViewModel) {

            if (!ModelState.IsValid) {
                return View(ViewModel);
            }
            
            ViewModel.Produto.valor = ViewModel.Produto.flagCortesia == true ? new decimal(0) : ViewModel.Produto.valor;
            
            bool flagSucesso = this.OProdutoBL.salvar(ViewModel.Produto, ViewModel.OImagem);

            if (flagSucesso){

                this.OProdutoTipoAssociadoBL.salvar(ViewModel.Produto.id, ViewModel.listaIdsTipoAssociado);

                this.Flash(UtilMessage.TYPE_MESSAGE_SUCCESS, UtilMessage.success("Sucesso", "Os dados foram salvos com sucesso."));

                return RedirectToAction("editar", new { id = ViewModel.Produto.id });
            }

            this.Flash(UtilMessage.TYPE_MESSAGE_ERROR, UtilMessage.error("Falha", "Não foi possível salvar as informações, tente novamente."));

            return View(ViewModel);

        }

        //Aucomplete para busca de produtos
        public ActionResult autocompletar(string term) {

            var listProdutos = this.OProdutoBL.autocompletar(term);

            return Json(listProdutos, JsonRequestBehavior.AllowGet);
        }


        //Buscar produto por ID
        public ActionResult buscar(int idProduto) {

            var OProduto = this.OProdutoBL.carregar(idProduto);

            if (OProduto == null) {
                return Json(new { error = true, message = "O produto informado não foi localizado." }, JsonRequestBehavior.AllowGet);
            }
            
            var Retorno = new {
                OProduto.id,
                OProduto.nome,
                OProduto.valor,
                valorFormatado = OProduto.valor.ToString("C"),
                OProduto.flagSomenteAssociadosQuites,
                OProduto.flagCortesia,
                OProduto.peso,
                pesoFormatado = OProduto.peso.ToString("0.000"),
                OProduto.percentualDescontoAssociado,
                OProduto.ativo,
                OProduto.flagValorConfiguravel,
                OProduto.valorGanhoDiario,
                OProduto.qtdeDiasDuracao,
            };
            
            return Json(Retorno, JsonRequestBehavior.AllowGet);
        }

        //
        [HttpPost, ActionName("alterar-status")]
        public ActionResult alterarStatus(int id) {
            return Json(this.OProdutoBL.alterarStatus(id));
        }

        ///Exclusao de Produtos
        [HttpPost]
        public ActionResult excluir(int[] id) {

            var Retorno = new JsonMessage();
            Retorno.error = false;
            Retorno.message = "Os registros informados foram removidos com sucesso.";

            foreach (int idExclusao in id) {

                var ORetornoExclusao = this.OProdutoBL.excluir(idExclusao);

                if (ORetornoExclusao.flagError) {
                    Retorno.error = true;
                    Retorno.message = ORetornoExclusao.listaErros.FirstOrDefault();
                }

            }

            return Json(Retorno);
        }

        ///Exclusao de itens do Produto
        [HttpPost]
        public ActionResult excluirComposicao(int[] id) {

            var Retorno = new JsonMessage();
            Retorno.error = false;
            Retorno.message = "Os registros informados foram removidos com sucesso.";

            foreach (int idExclusao in id) {

                var ORetornoExclusao = this.OProdutoComposicaoExclusaoBL.excluir(idExclusao);

                if (ORetornoExclusao.flagError) {
                    Retorno.error = true;
                    Retorno.message = ORetornoExclusao.listaErros.FirstOrDefault();
                }

            }

            return Json(Retorno);
        }


    }
}