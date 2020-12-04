using System;
using System.Web.Mvc;
using BLL.FinanceiroLancamentos;
using WEB.App_Infrastructure;
using System.Linq;
using PagedList;

namespace WEB.Areas.FinanceiroLancamentos.Controllers{

    [OrganizacaoFilter]
    public class CredorListaController : BaseSistemaController{

        //Atributos
        private ICredorBL _CredorBL;
        private CredorVWBL _CredorVWBL;

        //Propriedades
        private ICredorBL OCredorBL => _CredorBL = _CredorBL ?? new CredorBL();
        private CredorVWBL OCredorVWBL => _CredorVWBL = _CredorVWBL ?? new CredorVWBL();

        public ActionResult index(){
            
            var valorBusca = UtilRequest.getString("valorBusca");

            var ativo = UtilRequest.getBool("flagAtivo");

            var listaCredores = this.OCredorBL.listar(valorBusca, ativo).OrderBy(x => x.Pessoa.nome)
                                    .ToPagedList(UtilRequest.getNroPagina(), UtilRequest.getNroRegistros());

            return View(listaCredores);

        }
        
        /// <summary>
        /// Retorna as informações do credor, quando selecionado em um combo
        /// </summary>
        [ActionName("autocomplete-informacoes-credor"), HttpPost]
        public JsonResult autocompleteInformacoesCredor(string id) {
            if (string.IsNullOrEmpty(id)) {
                return Json(new { error = true, message = "Parâmetro de busca não informado" });
            }

            var array = id.Split('#');
            var flagCategoriaPessoa = array[0];
            var idPessoa = Convert.ToInt32(array[1]);

            var OCredor = OCredorVWBL.listar("").FirstOrDefault(x => x.flagCategoriaPessoa == flagCategoriaPessoa && x.idPessoa == idPessoa);

            if (OCredor == null) {
                return Json(new { error = true, message = "Não foi possível localizar os dados do credor" });
            }

            return Json(new { error = false, OCredor.nroDocumento, nroTelefone = OCredor.nroTelPrincipal, OCredor.idPessoa, id = OCredor.flagCategoriaPessoa + "#" + OCredor.idPessoa, text = (OCredor.descricaoCategoriaPessoa.ToUpper() + " - " + OCredor.nome.ToUpper() + " (" + UtilString.formatCPFCNPJ(OCredor.nroDocumento) + ")") });
        }
        
        [ActionName("auto-complete-credor"), HttpGet]
        public ActionResult autoCompleteCredor() {
            
            var valorBusca = UtilRequest.getString("q");
            var page = UtilRequest.getInt32("page");

            page = page > 0 ? page : 1;

            var lista = this.OCredorVWBL.listar(valorBusca).OrderBy(x => x.nome).ToPagedList(page, 30);

            var listaJson = lista.Select(x => new {id = x.flagCategoriaPessoa + "#" + x.idPessoa, text = (x.descricaoCategoriaPessoa.ToUpper() + " - " + x.nome.ToUpper() + (x.nroDocumento.isEmpty() ? "" : " (" + UtilString.formatCPFCNPJ(x.nroDocumento) + ")")) }).ToList();
            
            return Json(new {items = listaJson, page = page, total_count = lista.TotalItemCount}, JsonRequestBehavior.AllowGet);
        }
    }

}