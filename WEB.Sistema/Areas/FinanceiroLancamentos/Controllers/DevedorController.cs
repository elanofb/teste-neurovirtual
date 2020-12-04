using System;
using System.Linq;
using System.Web.Mvc;
using BLL.FinanceiroLancamentos;
using DAL.FinanceiroLancamentos;
using DAL.Pessoas;
using PagedList;
using WEB.App_Infrastructure;
using WEB.Areas.FinanceiroLancamentos.ViewModels;

namespace WEB.Areas.FinanceiroLancamentos.Controllers{

    public class DevedorController : BaseSistemaController{

        //Atributos
        private IDevedorBL _DevedorBL;
        private IDevedorVWBL _DevedorVWBL;

        //Propriedades
        private IDevedorBL ODevedorBL => (this._DevedorBL = this._DevedorBL ?? new DevedorBL());
        private IDevedorVWBL ODevedorVWBL => (this._DevedorVWBL = this._DevedorVWBL ?? new DevedorVWBL());

        [ActionName("modal-cadastrar-devedor"), HttpGet]
        public ActionResult modalCadastrarDevedor(int? id){

            var ViewModel = new DevedorForm();

            ViewModel.Devedor = this.ODevedorBL.carregar(UtilNumber.toInt32(id)) ?? new Devedor();

            ViewModel.Devedor.Pessoa = ViewModel.Devedor.Pessoa ?? new Pessoa();

            ViewModel.group = UtilRequest.getString("group");

            return View(ViewModel);
        }

        [ActionName("salvar-modal-devedor"), HttpPost]
        public ActionResult salvarDevedor(DevedorForm ViewModel) {

            if (!ModelState.IsValid) {

                ViewModel.Devedor.Pessoa = ViewModel.Devedor.Pessoa ?? new Pessoa();

                return PartialView("modal-cadastrar-devedor", ViewModel);
            }
            
            bool flagSucesso = this.ODevedorBL.salvar(ViewModel.Devedor);

            var nroDocumento = UtilString.formatCPFCNPJ(ViewModel.Devedor.Pessoa.nroDocumento);

            return Json(new {
                error = false,
                flagSucesso = flagSucesso,
                id = "DV#" + ViewModel.Devedor.Pessoa.id,
                descricao = "Devedor - " + ViewModel.Devedor.Pessoa.nome + (!nroDocumento.isEmpty() ? " (" + nroDocumento + ")" : ""),
                nroDocumento = ViewModel.Devedor.Pessoa.nroDocumento,
                nroTelefone = ViewModel.Devedor.Pessoa.nroTelPrincipal,
                group = ViewModel.group
            });
        }

        /// <summary>
        /// Retorna as informações do credor, quando selecionado em um combo
        /// </summary>
        [ActionName("autocomplete-informacoes-devedor"), HttpPost]
        public JsonResult autocompleteInformacoesDevedor(string id) {
            if (string.IsNullOrEmpty(id)) {
                return Json(new { error = true, message = "Parâmetro de busca não informado" });
            }

            var array = id.Split('#');
            var flagCategoriaPessoa = array[0];
            var idPessoa = Convert.ToInt32(array[1]);

            var ODevedor = ODevedorVWBL.listar("").FirstOrDefault(x => x.flagCategoriaPessoa == flagCategoriaPessoa && x.idPessoa == idPessoa);

            if (ODevedor == null) {
                return Json(new { error = true, message = "Não foi possível localizar os dados do devedor" });
            }

            return Json(new { error = false, ODevedor.nroDocumento, nroTelefone = ODevedor.nroTelPrincipal });
        }

        [ActionName("auto-complete-devedor"), HttpGet]
        public ActionResult autoCompleteDevedor(int? id) {

            var valorBusca = UtilRequest.getString("q");
            var page = UtilRequest.getInt32("page");

            page = page > 0 ? page : 1;

            var lista = this.ODevedorVWBL.listar(valorBusca).OrderBy(x => x.nome).ToPagedList(page, 30);

            var listaJson = lista.Select(x => new {id = x.flagCategoriaPessoa + "#" + x.idPessoa, text = (x.descricaoCategoriaPessoa.ToUpper() + " - " + x.nome.ToUpper() + " (" + UtilString.formatCPFCNPJ(x.nroDocumento) + ")") }).ToList();

            return Json(new {items = listaJson, page = page, total_count = lista.TotalItemCount}, JsonRequestBehavior.AllowGet);
        }
    }
}