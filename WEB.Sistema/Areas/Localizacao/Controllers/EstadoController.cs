using BLL.Localizacao;
using DAL.Localizacao;
using MvcFlashMessages;
using PagedList;
using System;
using System.Json;
using System.Linq;
using System.Web.Mvc;
using WEB.App_Infrastructure;
using WEB.Areas.Localizacao.ViewModels;

namespace WEB.Areas.Localizacao.Controllers {

    public class EstadoController : BaseSistemaController {

        private IEstadoBL _IEstadoBL { get; set; }

        private IEstadoBL OEstadoBL => _IEstadoBL = _IEstadoBL ?? new EstadoBL();

		public EstadoController() { 

		} 

		//
        public ActionResult listar() {

			string ativo = UtilRequest.getString("flagStatus");
			string valorBusca = UtilRequest.getString("valorBusca");

			var queryEstados = this.OEstadoBL.listar(valorBusca, ativo);
            
			queryEstados = queryEstados.OrderBy(x => x.sigla);
			return View(queryEstados.ToPagedList(UtilRequest.getNroPagina(), UtilRequest.getNroRegistros()));

        }
   

        //
		[HttpGet]
        public ActionResult editar(int? id) {

			var ViewModel = new EstadoVM();
			ViewModel.Estado = this.OEstadoBL.carregarPorId(UtilNumber.toInt32(id)) ?? new Estado();
            
            return View(ViewModel);
        }

        //
		[HttpPost]
        public ActionResult editar(EstadoVM ViewModel) {

			if(!ModelState.IsValid){
				return View(ViewModel);
			}
            
			bool flagSucesso = this.OEstadoBL.salvar(ViewModel.Estado);

			if (flagSucesso) {
                this.Flash(UtilMessage.TYPE_MESSAGE_SUCCESS, UtilMessage.success("Sucesso", "Os dados do estado foram salvos com sucesso."));
			    return RedirectToAction("listar");	
			}
            
            this.Flash(UtilMessage.TYPE_MESSAGE_ERROR, UtilMessage.error("Falha", "Houve um problema ao salvar o registro. Tente novamente."));
            return View(ViewModel);

        }    
	
        //
		[HttpPost]
		[ActionName("alterar-status")]
		public ActionResult alterarStatus(int id) {
			return Json(this.OEstadoBL.alterarStatus(id));
		}

        //
        [HttpPost]
        public ActionResult excluir(int[] id) {
			
            var Retorno = new JsonMessage();

            Retorno.error = false;

            Retorno.message = "Os registros informados foram removidos com sucesso.";

            foreach (var idEstado in id) {

                var RetornoItem = this.OEstadoBL.excluir(idEstado);

                if (RetornoItem.flagError) {
                    Retorno.error = true;
                    Retorno.message = "Não foi possível remover todos os registros.";
                }

            }

            return Json(Retorno);

        }

    }
}