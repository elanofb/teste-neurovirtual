using BLL.AssociadosCarteireinha;
using DAL.AssociadosCarteirinha;
using System;
using System.Json;
using System.Linq;
using System.Web.Mvc;
using MvcFlashMessages;
using WEB.App_Infrastructure;
using WEB.Areas.AssociadosCarteirinha.ViewModels;

namespace WEB.Areas.AssociadosCarteirinha.Controllers {

    public class AssociadoCarteirinhaController : BaseSistemaController{

		//Atributos
		private IAssociadoCarteirinhaBL _AssociadoCarteirinhaBL; 

		//Propriedades
        private IAssociadoCarteirinhaBL OAssociadoCarteirinhaBL { get { return (this._AssociadoCarteirinhaBL = this._AssociadoCarteirinhaBL ?? new AssociadoCarteirinhaBL()); } }

        //Construtor
        public AssociadoCarteirinhaController() {

        }

		//Bloco Partial para histórico de envio de carteirinha
		[HttpGet, ActionName("partial-listar-historico-envio")]
        public PartialViewResult partialListarRelacionamentos(int idAssociado){
			var listaOcorrencias = this.OAssociadoCarteirinhaBL.listar(idAssociado).OrderByDescending(x => x.dtEnvio).ToList();
            return PartialView(listaOcorrencias);
        }

		//Formulário Parcial para nova ocorrência de envio de carteirinha
		[HttpGet]
		public PartialViewResult editar(int? id, int? idAssociado){

			var OAssociadoCarteirinha = this.OAssociadoCarteirinhaBL.carregar(UtilNumber.toInt32(id)) ?? new AssociadoCarteirinha();
			OAssociadoCarteirinha.idAssociado = ( OAssociadoCarteirinha.idAssociado > 0 ? OAssociadoCarteirinha.idAssociado : UtilNumber.toInt32(idAssociado));

			AssociadosCarteirinhaForm ViewModel = new AssociadosCarteirinhaForm();
			ViewModel.AssociadoCarteirinha = OAssociadoCarteirinha;

			return PartialView(ViewModel);
		}


		//Formulario submetido para nova ocorrência
		[HttpPost]
		public ActionResult editar(AssociadosCarteirinhaForm ViewModel){

			if (!ModelState.IsValid) {
				return PartialView(ViewModel);
			}

			bool flagSalvo = this.OAssociadoCarteirinhaBL.salvar(ViewModel.AssociadoCarteirinha);

            if (flagSalvo) {

                this.Flash(UtilMessage.TYPE_MESSAGE_SUCCESS, UtilMessage.success("Sucesso", "Os dados foram salvos com sucesso."));

            } else {

                this.Flash(UtilMessage.TYPE_MESSAGE_ERROR, UtilMessage.error("Falha", "Não foi possível salvar as informações, tente novamente."));

            }

			return Json(new{ flagSucesso = flagSalvo }, JsonRequestBehavior.AllowGet);
		}

        //Excluir determinado registro
        [HttpPost]
        public ActionResult excluir(int[] id) {
			JsonMessage Retorno = new JsonMessage();
			Retorno.error = false;

			foreach (int idExclusao in id) { 
				UtilRetorno RetornoExclusao = this.OAssociadoCarteirinhaBL.excluir(idExclusao);
				
				if (RetornoExclusao.flagError) { 
					Retorno.error = false;
				}
			}

            return Json(Retorno);
        }
    }
}
