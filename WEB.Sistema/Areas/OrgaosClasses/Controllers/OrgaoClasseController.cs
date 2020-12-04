using System;
using System.Json;
using System.Linq;
using System.Web.Mvc;
using BLL.OrgaosClasses;
using DAL.OrgaosClasses;
using DAL.Permissao.Security.Extensions;
using MvcFlashMessages;
using PagedList;
using WEB.Areas.OrgaosClasses.ViewModels;

namespace WEB.Areas.OrgaosClasses.Controllers {

	public class OrgaoClasseController : Controller {

		//Constantes

		//Atributos
		private IOrgaoClasseBL _OrgaoClasseBL;

		//Propriedades
		private IOrgaoClasseBL OOrgaoClasseBL => _OrgaoClasseBL = _OrgaoClasseBL ?? new OrgaoClasseBL();

	    //Construtor
		public OrgaoClasseController() { 
				
		}

		//
		public ActionResult listar() {

			string descricao = UtilRequest.getString("valorBusca");

            bool? ativo = UtilRequest.getBool("flagAtivo");

            var listaRegistros = this.OOrgaoClasseBL.listar(descricao, ativo).OrderBy(x => x.descricao);

            return View(listaRegistros.ToPagedList(UtilRequest.getNroPagina(), UtilRequest.getNroRegistros()));
		}

		//
		[HttpGet]
		public ActionResult editar(int? id) {

			OrgaoClasseForm ViewModel = new OrgaoClasseForm();

			var Oregistro = this.OOrgaoClasseBL.carregar(UtilNumber.toInt32(id)) ?? new OrgaoClasse();

		    ViewModel.OrgaoClasse = Oregistro;

			return View(ViewModel);
		}

		//
		[HttpPost]
		public ActionResult editar(OrgaoClasseForm ViewModel) {

			if (!ModelState.IsValid) {
				return View(ViewModel);
			}

			bool flagSucesso = this.OOrgaoClasseBL.salvar(ViewModel.OrgaoClasse);

			if (flagSucesso) {

                this.Flash(UtilMessage.TYPE_MESSAGE_SUCCESS, UtilMessage.success("Sucesso!", "Os dados foram salvos com sucesso."));

				return RedirectToAction("editar", new { id = ViewModel.OrgaoClasse.id });
			}

            this.Flash(UtilMessage.TYPE_MESSAGE_ERROR, UtilMessage.error("Erro!", "Houve um problema ao salvar o registro. Tente novamente."));

            return View(ViewModel);
		}

        //
		[HttpPost, ActionName("alterar-status")]
		public ActionResult alterarStatus(int id) {
			return Json(this.OOrgaoClasseBL.alterarStatus(id));
		}

		//
		[HttpPost]
		public ActionResult excluir(int[] id) {

            var Retorno = new JsonMessage();
		    Retorno.error = false;
		    Retorno.message = "Os registros informados foram removidos com sucesso.";

		    foreach (var idItem in id) {

		        var RetornoItem = this.OOrgaoClasseBL.excluir(idItem, User.id());

		        if (RetornoItem.flagError == true) {
		            Retorno.error = true;
		            Retorno.message = "Nem todos os registros puderam ser removidos. Tente novamente mais tarde.";
		        }
		    }

			return Json(Retorno);
		}
	}
}