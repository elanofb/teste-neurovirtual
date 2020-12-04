using System;
using System.Json;
using System.Linq;
using System.Web.Mvc;
using BLL.Cargos;
using WEB.Areas.Cargos.ViewModels;
using DAL.Cargos;
using DAL.Permissao.Security.Extensions;
using MvcFlashMessages;
using PagedList;

namespace WEB.Areas.Cargos.Controllers {

	public class CargoController : Controller {

		//Constantes

		//Atributos
		private ICargoBL _CargoBL;

		//Propriedades
		private ICargoBL OCargoBL => _CargoBL = _CargoBL ?? new CargoBL();

	    //Construtor
		public CargoController() { 
				
		}
        
		//
		public ActionResult listar() {

			string descricao = UtilRequest.getString("valorBusca");

            string ativo = UtilRequest.getString("flagAtivo");

            var listaCargo = this.OCargoBL.listar(descricao, ativo).OrderBy(x => x.descricao);

            return View(listaCargo.ToPagedList(UtilRequest.getNroPagina(), UtilRequest.getNroRegistros()));
		}

		//
		[HttpGet]
		public ActionResult editar(int? id) {

			CargoForm ViewModel = new CargoForm();

			var OCargo = this.OCargoBL.carregar(UtilNumber.toInt32(id)) ?? new Cargo();

		    ViewModel.Cargo = OCargo;

			return View(ViewModel);
		}

		//
		[HttpPost]
		public ActionResult editar(CargoForm ViewModel) {

			if (!ModelState.IsValid) {
				return View(ViewModel);
			}

			bool flagSucesso = this.OCargoBL.salvar(ViewModel.Cargo);

			if (flagSucesso) {

                this.Flash(UtilMessage.TYPE_MESSAGE_SUCCESS, UtilMessage.success("Sucesso!", "Os dados foram salvos com sucesso."));

				return RedirectToAction("editar", new { id = ViewModel.Cargo.id });
			}

            this.Flash(UtilMessage.TYPE_MESSAGE_ERROR, UtilMessage.error("Erro!", "Houve um problema ao salvar o registro. Tente novamente."));

            return View(ViewModel);
		}

        //
        [HttpPost, ActionName("alterar-status")]
        public ActionResult alterarStatus(int id) {
	        return Json(this.OCargoBL.alterarStatus(id));
        }

		//
		[HttpPost]
		public ActionResult excluir(int[] id) {

            var Retorno = new JsonMessage();
		    Retorno.error = false;
		    Retorno.message = "Os registros informados foram removidos com sucesso.";

		    foreach (var idItem in id) {

		        var RetornoItem = this.OCargoBL.excluir(idItem, User.id());

		        if (RetornoItem.flagError == true) {
		            Retorno.error = true;
		            Retorno.message = "Algum(ns) registro(s) não pode(ram) ser removido(s).";
		        }
		    }

			return Json(Retorno);
		}
	}
}