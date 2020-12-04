using System;
using System.Linq;
using System.Web.Mvc;
using PagedList;
using System.Json;
using BLL.Publicacoes;
using DAL.Publicacoes;
using MvcFlashMessages;
using WEB.Areas.Publicacoes.ViewModels;

namespace WEB.Areas.Publicacoes.Controllers {

	public class TipoGaleriaFotoController : Controller {

		//Atributos
		private ITipoGaleriaFotoBL _TipoGaleriaFotoBL;

		//Propriedades
		private ITipoGaleriaFotoBL OTipoGaleriaFotoBL => _TipoGaleriaFotoBL = _TipoGaleriaFotoBL ?? new TipoGaleriaFotoBL();
		
		public ActionResult listar() {

            var descricao = UtilRequest.getString("valorBusca");
			var ativo = UtilRequest.getBool("flagAtivo");

			var listaTipoGaleriaFoto = this.OTipoGaleriaFotoBL.listar(descricao, ativo).OrderBy(x => x.descricao);

			return View(listaTipoGaleriaFoto.ToPagedList(UtilRequest.getNroPagina(), UtilRequest.getNroRegistros()));
		}

		[HttpGet]
		public ActionResult editar(int? id) {

			var ViewModel = new TipoGaleriaFotoForm();
			ViewModel.TipoGaleriaFoto = this.OTipoGaleriaFotoBL.carregar(UtilNumber.toInt32(id)) ?? new TipoGaleriaFoto();

			return View(ViewModel);
		}

		[HttpPost]
		public ActionResult editar(TipoGaleriaFotoForm ViewModel) {

			if (!ModelState.IsValid) {
				return View(ViewModel);
			}

			bool flagSucesso = this.OTipoGaleriaFotoBL.salvar(ViewModel.TipoGaleriaFoto);
			
            if (flagSucesso) {

                this.Flash(UtilMessage.TYPE_MESSAGE_SUCCESS, UtilMessage.success("Sucesso!", "Os dados foram salvos com sucesso."));

                return RedirectToAction("editar", new { ViewModel.TipoGaleriaFoto.id });

            }

			this.Flash(UtilMessage.TYPE_MESSAGE_ERROR, UtilMessage.error("Erro!", "Houve um problema ao salvar o registro. Tente novamente."));
            
			return View(ViewModel);
		}

		[HttpGet, ActionName("modal-editar")]
		public ActionResult modalEditar(int? id) {
			
			var ViewModel = new TipoGaleriaFotoForm();

			ViewModel.TipoGaleriaFoto = this.OTipoGaleriaFotoBL.carregar(UtilNumber.toInt32(id)) ?? new TipoGaleriaFoto();

			return PartialView(ViewModel);
		}

		[HttpPost, ActionName("salvar-modal-editar")]
		public ActionResult salvarModalEditar(TipoGaleriaFotoForm ViewModel) {

			if (!ModelState.IsValid) {
				return PartialView("modal-editar", ViewModel);
			}

		    ViewModel.TipoGaleriaFoto.ativo = true;

			bool flagSucesso = this.OTipoGaleriaFotoBL.salvar(ViewModel.TipoGaleriaFoto);

			return Json(new {error = false, flagSucesso, ViewModel.TipoGaleriaFoto.id, ViewModel.TipoGaleriaFoto.descricao});
		}

		[HttpPost]
		public ActionResult excluir(int[] id) {
			JsonMessage Retorno = new JsonMessage();

			foreach (int idExclusao in id) { 
				bool flagSucesso = this.OTipoGaleriaFotoBL.excluir(idExclusao);

				if (!flagSucesso) { 
					Retorno.error = true;
					Retorno.message = "Alguns registros não puderam ser excluídos.";
				}
			}

		    if (Retorno.error == false) {
		        Retorno.error = false;
		        Retorno.message = "Os registros foram excluídos com sucesso.";
		    }

            return Json(Retorno);
		}

        [ActionName("alterar-status"), HttpPost]
        public ActionResult alterarStatus(int id) {
            return Json(this.OTipoGaleriaFotoBL.alterarStatus(id));
        }
	}
}