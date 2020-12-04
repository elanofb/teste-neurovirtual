using System;
using System.Json;
using System.Linq;
using System.Web.Mvc;
using BLL.RamosAtividade;
using DAL.Permissao.Security.Extensions;
using DAL.RamosAtividade;
using MvcFlashMessages;
using PagedList;
using WEB.Areas.RamosAtividade.ViewModels;

namespace WEB.Areas.RamosAtividade.Controllers {

	public class RamoAtividadeController : Controller {

		//Constantes

		//Atributos
		private IRamoAtividadeBL _RamoAtividadeBL;

		//Propriedades
		private IRamoAtividadeBL ORamoAtividadeBL => _RamoAtividadeBL = _RamoAtividadeBL ?? new RamoAtividadeBL();

	    //Construtor
		public RamoAtividadeController() { 
				
		}

		//
		public ActionResult listar() {

			string descricao = UtilRequest.getString("valorBusca");

            bool? ativo = UtilRequest.getBool("flagAtivo");

            var listaRegistros = this.ORamoAtividadeBL.listar(descricao, ativo).OrderBy(x => x.descricao);

            return View(listaRegistros.ToPagedList(UtilRequest.getNroPagina(), UtilRequest.getNroRegistros()));
		}

		//
		[HttpGet]
		public ActionResult editar(int? id) {

			RamoAtividadeForm ViewModel = new RamoAtividadeForm();

			var Oregistro = this.ORamoAtividadeBL.carregar(UtilNumber.toInt32(id)) ?? new RamoAtividade();

		    ViewModel.RamoAtividade = Oregistro;

			return View(ViewModel);
		}

		//
		[HttpPost]
		public ActionResult editar(RamoAtividadeForm ViewModel) {

			if (!ModelState.IsValid) {
				return View(ViewModel);
			}

			bool flagSucesso = this.ORamoAtividadeBL.salvar(ViewModel.RamoAtividade);

			if (flagSucesso) {

                this.Flash(UtilMessage.TYPE_MESSAGE_SUCCESS, UtilMessage.success("Sucesso!", "Os dados foram salvos com sucesso."));

				return RedirectToAction("editar", new { id = ViewModel.RamoAtividade.id });
			}

            this.Flash(UtilMessage.TYPE_MESSAGE_ERROR, UtilMessage.error("Erro!", "Houve um problema ao salvar o registro. Tente novamente."));

            return View(ViewModel);
		}

        //
		[HttpPost, ActionName("alterar-status")]
		public ActionResult alterarStatus(int id) {
			return Json(this.ORamoAtividadeBL.alterarStatus(id));
		}

		//
		[HttpPost]
		public ActionResult excluir(int[] id) {

            var Retorno = new JsonMessage();
		    Retorno.error = false;
		    Retorno.message = "Os registros informados foram removidos com sucesso.";

		    foreach (var idItem in id) {

		        var RetornoItem = this.ORamoAtividadeBL.excluir(idItem, User.id());

		        if (RetornoItem.flagError == true) {
		            Retorno.error = true;
		            Retorno.message = "Nem todos os registros puderam ser removidos. Tente novamente mais tarde.";
		        }
		    }

			return Json(Retorno);
		}
	}
}