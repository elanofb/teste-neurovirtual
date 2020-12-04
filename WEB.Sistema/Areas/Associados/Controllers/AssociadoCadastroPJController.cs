using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using BLL.Associados;
using BLL.Services;
using DAL.Associados;
using DAL.ConfiguracoesAssociados;
using DAL.Documentos;
using DAL.Permissao.Security.Extensions;
using DAL.Pessoas;
using MvcFlashMessages;
using WEB.Areas.Associados.ViewModels;
using DAL.Entities;

namespace WEB.Areas.Associados.Controllers {

    public class AssociadoCadastroPJController : Controller {

        //Atributos
        private IAssociadoCadastroBL _AssociadoCadastroBL;
        private IAssociadoBL _AssociadoBL;
        private IMembroSaldoConsultaBL _SaldoConsultaBL;


        //Propriedades
        private IAssociadoCadastroBL OAssociadoCadastroBL => _AssociadoCadastroBL = _AssociadoCadastroBL ?? new AssociadoCadastroBL();
        private IAssociadoBL OAssociadoBL => _AssociadoBL = _AssociadoBL ?? new AssociadoBL();
        private IMembroSaldoConsultaBL OSaldoConsultaBL => _SaldoConsultaBL = _SaldoConsultaBL ?? new MembroSaldoConsultaBL();

        // GET: Associados/AssociadoCadastroPJ
        [OrganizacaoFilter]
        public ActionResult cadastrar() {

            var idTipoAssociado = UtilRequest.getInt32("idTipoAssociado");

            var ViewModel = new AssociadoCadastroPJForm();

            ViewModel.Associado = new Associado() { idTipoAssociado = idTipoAssociado };

            ViewModel.Associado.Pessoa = new Pessoa();

            ViewModel.carregarConfiguracoes();

            ViewModel.carregaDados();

            return View("editar", ViewModel);
        }

        /// <summary>
        /// 
        /// </summary>
        [OrganizacaoFilter]
        public ActionResult editar(int? id) {

            var ViewModel = new AssociadoCadastroPJForm();

            ViewModel.Associado = this.OAssociadoBL.carregar(id.toInt());

            ViewModel.Saldo = this.OSaldoConsultaBL.query(id.toInt())
                                  .Select(x => new { x.id, x.saldoAtual, x.dtAtualizacaoSaldo})
                                  .FirstOrDefault()
                                  .ToJsonObject<MembroSaldo>() ?? new MembroSaldo();           
            
            if (ViewModel.Associado == null){
                return RedirectToAction("cadastrar");
            }

            if (ViewModel.Associado.Pessoa.flagTipoPessoa == "F") {
                return RedirectToAction("editar", "AssociadoCadastroPF", new { ViewModel.Associado.id });
            }

            ViewModel.carregarConfiguracoes();

            ViewModel.carregaDados(true);

            ViewModel.Associado.Pessoa = ViewModel.Associado.Pessoa ?? new Pessoa();

            ViewModel.Associado.Pessoa.limparListas();
            ViewModel.Associado.limparListas();

            ViewModel.carregarValorCampos(ViewModel);
            
            ViewModel.carregarConfiguracaoMembro();

            return View("editar", ViewModel);
        }

        [ActionName("aba-dados-cadastrais")]
        public ActionResult abaDadosCadastrais(int? id) {

            var idTipoAssociado = UtilRequest.getInt32("idTipoAssociado");

            var ViewModel = new AssociadoCadastroPJForm();

            if (id > 0) {
                ViewModel.Associado = this.OAssociadoBL.listar(0, "", "", "").Where(x => x.id == id)
                                                        .Include(x => x.UsuarioCadastro)
                                                        .Include(x => x.Unidade)
                                                        .Include(x => x.Pessoa.CidadeOrigem).FirstOrDefault();

                if (ViewModel.Associado == null){
                    return RedirectToAction("cadastrar");
                }

                if (ViewModel.Associado.Pessoa.flagTipoPessoa == "F"){
                    return RedirectToAction("editar", "AssociadoCadastroPJ", new { ViewModel.Associado.id });
                }
            } else {
                ViewModel.Associado = new Associado() { idTipoAssociado = idTipoAssociado };

                ViewModel.Associado.Pessoa = new Pessoa();
            }

            ViewModel.carregarConfiguracoes();

            var flagEdicao = id > 0;

            ViewModel.carregaDados(flagEdicao);

            ViewModel.Associado.Pessoa = ViewModel.Associado.Pessoa ?? new Pessoa();

            ViewModel.Associado.Pessoa.limparListas();
            ViewModel.Associado.limparListas();

            ViewModel.carregarValorCampos(ViewModel);

            return View("aba-dados-cadastrais", ViewModel);
        }

        /// <summary>
        /// Processar e salvar os dados enviados pelo formulário
        /// </summary>
        [HttpPost, OrganizacaoFilter]
        public ActionResult salvarCadastro(AssociadoCadastroPJForm ViewModel) {

            var flagEdicao = ViewModel.Associado.id > 0;

            ViewModel.carregaDados(flagEdicao);

            ViewModel.listaCampos.bind(Request.Form);

            if (ViewModel.Associado.id == 1) {
                
                this.Flash(UtilMessage.TYPE_MESSAGE_ERROR, UtilMessage.error("Falha", "Os dados da conta principal do sistema não podem ser editados."));

                return View("aba-dados-cadastrais", ViewModel);                
            }

            if (!ModelState.IsValid || ViewModel.listaCampos.Any(x => x.flagValidado == false)) {

                this.Flash(UtilMessage.TYPE_MESSAGE_ERROR, UtilMessage.error("Falha", "Algumas informações não passaram na validação, verifique abaixo."));

                return View("aba-dados-cadastrais", ViewModel);
            }

            var dbAssociado = this.OAssociadoBL.carregar(ViewModel.Associado.id);

            if (dbAssociado != null && dbAssociado.idOrganizacao != User.idOrganizacao()) {

                this.Flash(UtilMessage.TYPE_MESSAGE_ERROR, UtilMessage.error("Falha", "Você não tem permissão para acessar esse cadastro."));

                return View("aba-dados-cadastrais", ViewModel);

            }

            ViewModel = ViewModel.atribuirValoresFixos(ViewModel);

            ViewModel.Associado.Pessoa.flagTipoPessoa = "J";

            ViewModel.Associado.Pessoa.idTipoDocumento = TipoDocumentoConst.CNPJ;

            ViewModel.Associado.ativo = "E"; //Em admissão

            ViewModel.Associado.idOrigem = OrigemCadastroConst.SISTEMA;

            OAssociadoCadastroBL.salvar(ViewModel.Associado);

            if (ViewModel.Associado.id > 0) {

                this.Flash(UtilMessage.TYPE_MESSAGE_SUCCESS, UtilMessage.success("Sucesso", "Os dados do membro foram validados e salvos sem erros."));

                return Json(new { error = false, message = "Os dados foram salvos com sucesso.", urlRedirecionamento = Url.Action("editar", new { ViewModel.Associado.id }) });
            }

            this.Flash(UtilMessage.TYPE_MESSAGE_ERROR, UtilMessage.error("Falha", "Não foi possível salvar os dados."));

            return View("aba-dados-cadastrais", ViewModel);
        }
    }
}