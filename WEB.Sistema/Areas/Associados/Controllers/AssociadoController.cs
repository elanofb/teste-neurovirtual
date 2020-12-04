using BLL.Associados;
using PagedList;
using System;
using System.Linq;
using System.Web.Mvc;
using DAL.Associados.DTO;
using WEB.Helpers;

namespace WEB.Areas.Associados.Controllers {

    public class AssociadoController : Controller {

        //Atributos
        private IAssociadoBL _AssociadoBL;
        
        //Propriedades
        private IAssociadoBL OAssociadoBL => _AssociadoBL = _AssociadoBL ?? new AssociadoBL();
        
        //Listagem dos associados do sistema
        public ActionResult listar() {

            var idTipoAssociado = UtilRequest.getInt32("idTipoAssociado");
            var flagDocumentosAprovados = UtilRequest.getBool("flagDocumentosAprovados");
            var valorBusca      = UtilRequest.getString("valorBusca");
            var ativo           = UtilRequest.getString("flagAtivo");
            var flagTipoSaida   = UtilRequest.getString("tipoSaida");
            var flagSituacaoContribuicao = UtilRequest.getString("flagSituacaoContribuicao");

            var queryAssociados = this.OAssociadoBL.listar(idTipoAssociado, flagSituacaoContribuicao, valorBusca, ativo)
                                                   .Where(x => x.ativo != "E");
            
            if(flagDocumentosAprovados == true) {
                queryAssociados = queryAssociados.Where(associado => associado.dtAprovacaoDocumento != null);
            }
            
            if(flagDocumentosAprovados == false) {
                queryAssociados = queryAssociados.Where(associado => associado.dtAprovacaoDocumento == null);
            }
            
            if (flagTipoSaida == TipoSaidaHelper.EXCEL) {

                //var OAssociadoConsultaExportacao = new AssociadoConsultaExportacao();

                //OAssociadoConsultaExportacao.baixarExcel(listaAssociados.ToList());

                //return null;
            }

            var query = queryAssociados.Select(
                x => new ItemListaAssociado {
                    id                     = x.id,
                    nroAssociado           = x.nroAssociado,
                    descricaoTipoAssociado = x.TipoAssociado.nomeDisplay,
                    flagTipoPessoa         = x.Pessoa.flagTipoPessoa,
                    nome                   = x.Pessoa.nome,
                    razaoSocial            = x.Pessoa.razaoSocial,
                    nroDocumento           = x.Pessoa.nroDocumento,
                    nroTelefone            = x.Pessoa.listaTelefones.FirstOrDefault(y => y.dtExclusao == null && y.ativo == true).nroTelefone,
                    email                  = x.Pessoa.listaEmails.FirstOrDefault(y => y.dtExclusao == null).email,
                    dtCadastro             = x.dtCadastro,
                    dtAdmissao             = x.dtAdmissao,
                    ativo                  = x.ativo
                }
            );

            query = query.OrderBy(x => x.nome);

            var lista = query.ToPagedList(UtilRequest.getNroPagina(), UtilRequest.getNroRegistros());

            return View(lista);
        }
        

        //Lista de associados para aucomplete de campos
        public ActionResult autocomplete(string term) {

            var listAssociados = this.OAssociadoBL.autocompletar(term, 0);

            return Json(listAssociados, JsonRequestBehavior.AllowGet);
        }

        //Lista de associados para aucomplete de campos
        public ActionResult autocompletarAssociado() {

            var valorBusca = UtilRequest.getString("valorBusca");
            var page = UtilRequest.getInt32("page");

            page = page > 0 ? page : 1;

            var lista = this.OAssociadoBL.autocompletar(valorBusca, 0).OrderBy(x => x.value).ToPagedList(page, 30);

            var listaJson = lista.Select(x => new { id = x.id, text = (x.value.ToUpper() + " (" + UtilString.formatCPFCNPJ(x.nroDocumento) + ")") }).ToList();

            return Json(new { items = listaJson, page = page, total_count = lista.TotalItemCount }, JsonRequestBehavior.AllowGet);

        }

        [ActionName("autocomplete-informacoes-associado"), HttpPost]
        public JsonResult autocompleteInformacoesAssociado(string id) {
            if (string.IsNullOrEmpty(id)) {
                return Json(new { error = true, message = "Parâmetro de busca não informado" });
            }

            var OAssociado = OAssociadoBL.carregar(id.toInt());

            if (OAssociado == null) {
                return Json(new { error = true, message = "Não foi possível localizar os dados do associado" });
            }

            return Json(new { error = false, OAssociado.idPessoa, value = OAssociado.Pessoa.nome, OAssociado.Pessoa.nroDocumento });
        }
    }
}
