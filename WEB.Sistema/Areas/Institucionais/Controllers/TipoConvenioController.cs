using System;
using System.Linq;
using System.Web.Mvc;
using System.Json;
using BLL.Institucionais;
using DAL.Institucionais;
using PagedList;
using WEB.Areas.Institucionais.ViewModels;
using DAL.Permissao.Security.Extensions;
using MvcFlashMessages;

namespace WEB.Areas.Institucionais.Controllers {

	public class TipoConvenioController : Controller {
		//Constantes

		//Atributos
		private ITipoConvenioBL _TipoConvenioBL { get; set; }
		//Propriedades
		private ITipoConvenioBL OTipoConvenioBL => (this._TipoConvenioBL = this._TipoConvenioBL ?? new TipoConvenioBL());

	    public TipoConvenioController() {

		}

		//
		public ActionResult listar() {
			string nome = UtilRequest.getString("valorBusca");
			bool? ativo = UtilRequest.getBool("flagAtivo");

			var lista = this.OTipoConvenioBL.listar(nome, ativo).OrderBy(x => x.descricao);
			return View(lista.ToPagedList(UtilRequest.getNroPagina(), UtilRequest.getNroRegistros()));
		}

		[HttpGet]
		public ActionResult editar(int? id) {
			var ViewModel = new TipoConvenioForm();
			ViewModel.TipoConvenio = this.OTipoConvenioBL.carregar(UtilNumber.toInt32(id)) ?? new TipoConvenio();

			return View(ViewModel);
		}

		//

		[HttpPost, ValidateInput(false)]
		public ActionResult editar(TipoConvenioForm ViewModel) {

			if (!ModelState.IsValid) {
				return View(ViewModel);
			}

			bool flagSucesso = this.OTipoConvenioBL.salvar(ViewModel.TipoConvenio);

			if (flagSucesso) {

                this.Flash(UtilMessage.TYPE_MESSAGE_SUCCESS, UtilMessage.success("Sucesso", "Os dados foram salvos com sucesso."));

				return RedirectToAction("listar");
			}

            this.Flash(UtilMessage.TYPE_MESSAGE_ERROR, UtilMessage.error("Falha", "Não foi possível salvar as informações, tente novamente."));

			return View(ViewModel);
		}


		[HttpGet, ActionName("modal-editar")]
		public ActionResult modalEditar(int? id) {
			
			var ViewModel = new TipoConvenioForm();
			
			ViewModel.TipoConvenio = this.OTipoConvenioBL.carregar(UtilNumber.toInt32(id)) ?? new TipoConvenio();

			return PartialView(ViewModel);
		}

		[HttpPost, ActionName("salvar-modal-editar")]
		public ActionResult salvarModalEditar(TipoConvenioForm ViewModel) {

			if (!ModelState.IsValid) {
				return PartialView("modal-editar", ViewModel);
			}

			bool flagSucesso = this.OTipoConvenioBL.salvar(ViewModel.TipoConvenio);

			return Json(new {error = false, flagSucesso, ViewModel.TipoConvenio.id, ViewModel.TipoConvenio.descricao});
		}

		//
		[HttpPost]
        public ActionResult excluir(int[] id) {

            var Retorno = new JsonMessage();
            Retorno.error = false;
            Retorno.message = "Os registros informados foram removidos com sucesso.";

            foreach (var idItem in id) {

                var RetornoItem = this.OTipoConvenioBL.excluir(idItem, User.id());

                if (RetornoItem.flagError == true) {
                    Retorno.error = true;
                    Retorno.message = "Algum(ns) registro(s) não pode(ram) ser removido(s).";
                }
            }

            return Json(Retorno);
        }
    }
}