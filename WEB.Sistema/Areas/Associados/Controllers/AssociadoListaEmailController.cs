using System;
using System.Linq;
using System.Web.Mvc;
using WEB.App_Infrastructure;
using WEB.Areas.Associados.ViewModels;
using System.Json;
using BLL.Mailings;
using DAL.Mailings;
using MvcFlashMessages;

namespace WEB.Areas.Associados.Controllers{

    public class AssociadoListaEmailController : BaseSistemaController{

		//Atributos
		private IMailingBL _MailingBL; 

		//Propriedades
        private IMailingBL OMailingBL => _MailingBL = _MailingBL ?? new MailingBL();

		//
		[ActionName("partial-listar")]
        public PartialViewResult partialListar(){

            var valorBusca = UtilRequest.getString("valorBusca");
            var ativo = UtilRequest.getString("ativo");
            var idTipoMailing = UtilRequest.getInt32("idTipoMailing");
            var idAssociado = UtilRequest.getInt32("idAssociado");

            var lista = this.OMailingBL.listar(valorBusca, ativo, idTipoMailing, idAssociado).ToList();

            return PartialView(lista);
        }

		//
		[HttpGet]
		public PartialViewResult editar(int? id, int? idAssociado, int? idTipoAssociado) {

            var OMailing = this.OMailingBL.carregar(UtilNumber.toInt32(id)) ?? new Mailing();
            OMailing.idAssociado = ( OMailing.idAssociado > 0? OMailing.idAssociado : UtilNumber.toInt32(idAssociado));

            AssociadoListaEmailForm ViewModel = new AssociadoListaEmailForm();
			ViewModel.Mailing = OMailing;
			ViewModel.idTipoAssociado = Convert.ToInt32(idTipoAssociado);

			return PartialView(ViewModel);
		}


		//
		[HttpPost]
		public ActionResult editar(AssociadoListaEmailForm ViewModel){

			if (!ModelState.IsValid) {
				return PartialView(ViewModel);
			}

			bool flagSalvo = this.OMailingBL.salvar(ViewModel.Mailing);

            if (flagSalvo) {

                this.Flash(UtilMessage.TYPE_MESSAGE_SUCCESS, UtilMessage.success("Sucesso", "Os dados foram salvos com sucesso."));

            } else {

                this.Flash(UtilMessage.TYPE_MESSAGE_ERROR, UtilMessage.error("Falha", "Não foi possível salvar as informações, tente novamente."));

            }

			return Json(new{ flagSucesso = flagSalvo });
		}

        //Excluir determinado registro
        [HttpPost]
        public ActionResult excluir(int[] id) {
			JsonMessage Retorno = new JsonMessage();
			Retorno.error = false;

			foreach (int idExclusao in id) { 
				UtilRetorno RetornoExclusao = this.OMailingBL.excluir(idExclusao);
				
				if (RetornoExclusao.flagError) { 
					Retorno.error = false;
				}
			}

            return Json(Retorno);
        }
    }
}
