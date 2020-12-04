using System;
using System.Linq;
using System.Web.Mvc;
using BLL.Relacionamentos;
using DAL.Relacionamentos;
using WEB.Areas.Relacionamentos.ViewModels;
using PagedList;
using System.Json;
using MvcFlashMessages;

namespace WEB.Areas.Relacionamentos.Controllers {

    public class RelacionamentoController : Controller {

        // Constantes

        // Atributos
        private IOcorrenciaRelacionamentoPadraoBL _OcorrenciaRelacionamentoPadraoBL { get; set; }

        // Propriedades
        private IOcorrenciaRelacionamentoPadraoBL OOcorrenciaRelacionamentoPadraoBL { get { return (this._OcorrenciaRelacionamentoPadraoBL = this._OcorrenciaRelacionamentoPadraoBL ?? new OcorrenciaRelacionamentoPadraoBL()); } }


		//Listagem para consulta de Relacionamentos existentes
        public ActionResult listar() {
			string descricao = UtilRequest.getString("valorBusca");
			string ativo = UtilRequest.getString("flagAtivo");
            var listaOcorrencias = this.OOcorrenciaRelacionamentoPadraoBL.listar(descricao, ativo, "").OrderBy(x => x.descricao);

            return View( listaOcorrencias.ToPagedList(UtilRequest.getNroPagina(), UtilRequest.getNroRegistros()) );
        }


        /**
		*
		*/
		[HttpGet]
        public ActionResult editar(int? id) {
            OcorrenciaRelacionamentoForm ViewModel = new OcorrenciaRelacionamentoForm();

            ViewModel.OcorrenciaRelacionamento = this.OOcorrenciaRelacionamentoPadraoBL.carregar(UtilNumber.toInt32(id)) ?? new OcorrenciaRelacionamentoPadrao();
            return View(ViewModel);
        }

        /**
		*
		*/
		[HttpPost]
        public ActionResult editar(OcorrenciaRelacionamentoForm ViewModel){

			if(!ModelState.IsValid){
				return View(ViewModel);
			}

			bool flagSucesso = this.OOcorrenciaRelacionamentoPadraoBL.salvar(ViewModel.OcorrenciaRelacionamento);

            if (flagSucesso) {

                this.Flash(UtilMessage.TYPE_MESSAGE_SUCCESS, UtilMessage.success("Sucesso", "Os dados foram salvos com sucesso."));

            } else {

                this.Flash(UtilMessage.TYPE_MESSAGE_ERROR, UtilMessage.error("Falha", "Não foi possível salvar as informações, tente novamente."));

            }

			return RedirectToAction("editar", new { id = ViewModel.OcorrenciaRelacionamento.id });
        }    
	
        //Excluir um ou mais registros
        [HttpPost]
        public ActionResult excluir(int[] id) {
            JsonMessage Retorno = new JsonMessage();
			Retorno.error = false;

			foreach (int idExclusao in id) { 
				bool flagSucesso = this.OOcorrenciaRelacionamentoPadraoBL.excluir(idExclusao);
				
				if (!flagSucesso) { 
					Retorno.error = true;
					Retorno.message = "Algumas exclusões não puderam ser realizadas, tente novamente.";
				}
			}

            return Json(Retorno);
        }
	}
}