using System;
using System.Linq;
using System.Web.Mvc;
using PagedList;
using System.Json;
using BLL.MateriaisApoio;
using DAL.MateriaisApoio;
using WEB.Areas.MateriaisApoio.ViewModels;
using MvcFlashMessages;

namespace WEB.Areas.MateriaisApoio.Controllers {

	public class TipoMaterialApoioController : Controller {

		//Constantes

		//Atributos
		private ITipoMaterialApoioBL _ITipoMaterialApoioBL;

		//Propriedades
		private ITipoMaterialApoioBL OTipoMaterialApoioBL { get{ return (this._ITipoMaterialApoioBL = this._ITipoMaterialApoioBL ?? new TipoMaterialApoioBL()); }}
		

		/**
		*
		*/
		public TipoMaterialApoioController() { 
        } 

		/**
		*
		*/
		public ActionResult listar() {
			string descricao = UtilRequest.getString("valorBusca");
			string ativo = UtilRequest.getString("flagAtivo");
			var listaTipoProduto = this.OTipoMaterialApoioBL.listar(descricao, ativo).OrderBy(x => x.descricao);

			return View(listaTipoProduto.ToPagedList(UtilRequest.getNroPagina(), UtilRequest.getNroRegistros()));
		}

		[HttpGet]
		public ActionResult editar(int? id) {
			var ViewModel = new TipoMaterialApoioForm();
			ViewModel.TipoMaterialApoio = this.OTipoMaterialApoioBL.carregar(UtilNumber.toInt32(id)) ?? new TipoMaterialApoio();

			return View(ViewModel);
		}

		[HttpPost]
		public ActionResult editar(TipoMaterialApoioForm ViewModel) {
			if (!ModelState.IsValid) {
				return View(ViewModel);
			}

			bool flagSucesso = this.OTipoMaterialApoioBL.salvar(ViewModel.TipoMaterialApoio);
			
			if (flagSucesso) {

                this.Flash(UtilMessage.TYPE_MESSAGE_SUCCESS, UtilMessage.success("Sucesso", "Os dados foram salvos com sucesso."));

                return RedirectToAction("listar");
			}

            this.Flash(UtilMessage.TYPE_MESSAGE_ERROR, UtilMessage.error("Falha", "Não foi possível salvar as informações, tente novamente."));

			return View(ViewModel);
		}

        [ActionName("modal-cadastrar-categoria"), HttpGet]
        public ActionResult modalCadastrarCategoria(int? id) {

            var ViewModel = new TipoMaterialApoioForm();

            ViewModel.TipoMaterialApoio = this.OTipoMaterialApoioBL.carregar(UtilNumber.toInt32(id)) ?? new TipoMaterialApoio();
            
            return View(ViewModel);
        }

        [ActionName("salvar-modal-categoria"), HttpPost]
        public ActionResult salvarCategoria(TipoMaterialApoioForm ViewModel) {

            if (!ModelState.IsValid) {
                
                return PartialView("modal-cadastrar-categoria", ViewModel);
            }
            
            bool flagSucesso = this.OTipoMaterialApoioBL.salvar(ViewModel.TipoMaterialApoio);
            
            return Json(new {
                error = false,
                flagSucesso = flagSucesso,
                id = ViewModel.TipoMaterialApoio.id,
                descricao = ViewModel.TipoMaterialApoio.descricao
            });
        }

        //
        [HttpPost]
		public ActionResult excluir(int[] id) {
			JsonMessage Retorno = new JsonMessage();
			Retorno.error = false;

			foreach (int idExclusao in id) { 
				var ORetorno = this.OTipoMaterialApoioBL.excluir(idExclusao);

				if (ORetorno.flagError) { 
					Retorno.error = true;
					Retorno.message = "Alguns registros não puderam ser excluídos.";
				}
			}

			return Json(Retorno);
		}
	}
}