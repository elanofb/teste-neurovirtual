using System;
using System.Linq;
using System.Web.Mvc;
using BLL.Associados;
using DAL.Associados;
using System.Json;
using DAL.Permissao.Security.Extensions;
using MvcFlashMessages;
using WEB.Areas.Associados.ViewModels;

namespace WEB.Areas.Associados.Controllers {

	public class AssociadoAcaoController : Controller {

		//Atributos
		private IAssociadoBL _AssociadoBL;
		private IAssociadoAcaoBL _AssociadoAcaoBL;
        private IAssociadoEnvioFichaBL _AssociadoEnvioFichaBL;

		//Propriedades
		private IAssociadoBL OAssociadoBL => _AssociadoBL = _AssociadoBL ?? new AssociadoBL();
		private IAssociadoAcaoBL OAssociadoAcaoBL => _AssociadoAcaoBL = _AssociadoAcaoBL ?? new AssociadoAcaoBL();
        private IAssociadoEnvioFichaBL OAssociadoEnvioFichaBL => _AssociadoEnvioFichaBL = _AssociadoEnvioFichaBL ?? new AssociadoEnvioFichaBL();

	    //Abertura do modal para configurar a admissao do associado
		[ActionName("partial-admissao-associado"), HttpGet]
		public PartialViewResult partialAdmissaoAssociado(int idAssociado) {
            
			Associado OAssociado = this.OAssociadoBL.carregar(idAssociado);

			return PartialView(OAssociado);
		}

		//Salvar os dados de admissao do associado
		[ActionName("admitir-associado"), HttpPost]
		public ActionResult admitirAssociado(Associado ViewModel) {
            
			Associado OAssociado = this.OAssociadoBL.carregar(ViewModel.id);

			if (OAssociado == null) { 
				ModelState.AddModelError("", "O associado não pôde ser localizado.");
				return PartialView("partial-admissao-associado", ViewModel);
			}

			if (OAssociado.dtAdmissao.HasValue) { 
				ModelState.AddModelError("", "Esse associado já foi admitido.");
				return PartialView("partial-admissao-associado", ViewModel);
			}

			if (!ViewModel.dtAdmissao.HasValue || ViewModel.dtAdmissao.Value.Date > DateTime.Today) { 
				ModelState.AddModelError("dtAdmissao", "Informe uma data válida.");
				return PartialView("partial-admissao-associado", ViewModel);
			}

			this.OAssociadoAcaoBL.admitirAssociado(ViewModel.id, ViewModel.dtAdmissao);
			this.Flash(UtilMessage.TYPE_MESSAGE_SUCCESS, "O associado foi admitido com sucesso!");
			return Json( new{ flagSucesso = true} );
		}
        
		//Abertura do modal para configurar a desativacao do associado
		[ActionName("partial-bloquear-associado"), HttpGet]
		public PartialViewResult partialBloquearAssociado(int idAssociado) {
            
			Associado OAssociado = this.OAssociadoBL.carregar(idAssociado);

			return PartialView(OAssociado);
		}
        
		[ActionName("partial-inadimplencia-associado"), HttpGet]
		public PartialViewResult partialInadimplenciaAssociado(int idAssociado) {
            
			Associado OAssociado = this.OAssociadoBL.carregar(idAssociado);

			return PartialView(OAssociado);
		}
        
        //Reenvio de senha do associado
        [HttpPost, ActionName("enviar-link-senha")]
        public ActionResult enviarLinkSenha(int idAssociado) {

            var Retorno = this.OAssociadoAcaoBL.enviarLinkSenha(idAssociado);

            return Json(new JsonMessage { error = Retorno.flagError, message = Retorno.listaErros.FirstOrDefault() });
        }



        //Bloquear o associado
		[ActionName("bloquear-associado"), HttpPost]
		public ActionResult bloquearAssociado(Associado ViewModel, string observacoes) {
            
			Associado OAssociado = this.OAssociadoBL.carregar(ViewModel.id);

			if (OAssociado == null) { 
				ModelState.AddModelError("", "O associado não pôde ser localizado.");
				return PartialView("partial-bloquear-associado", ViewModel);
			}

			if (OAssociado.ativo == "N") { 
				ModelState.AddModelError("", "Esse associado já está desativado.");
				return PartialView("partial-bloquear-associado", ViewModel);
			}

			if (String.IsNullOrEmpty(observacoes)) { 
				ModelState.AddModelError("observacoes", "Informe o motivo para bloqueio.");
				return PartialView("partial-bloquear-associado", ViewModel);
			}

			this.OAssociadoAcaoBL.bloquearAssociado(ViewModel.id, observacoes);
			this.Flash(UtilMessage.TYPE_MESSAGE_SUCCESS, "O associado foi bloqueado com sucesso!");
			return Json( new{ flagSucesso = true} );
		}

        //Alterar o tipo do associado
        [ActionName("alterar-tipo-associado"), HttpPost]
	    public ActionResult alterarTipoAssociado() {

            var idAssociado = UtilRequest.getInt32("pk");

            var idTipoAssociado = UtilRequest.getInt32("value");

            if (idTipoAssociado == 0) {

                return Json(new { error = true, message = "O tipo de associado não foi informado." });

            }

            if (idAssociado == 0) {

                return Json(new { error = true, message = "O Associado não foi informado." });

            }

            var Retorno = this.OAssociadoAcaoBL.alterarTipo(idAssociado, idTipoAssociado, User.id());

            if (!Retorno.flagError) {
                return Json(new { error = false});
            }

            return Json(new { error = true, message = "Não foi possível salvar o registro.<br/>"+Retorno.listaErros.FirstOrDefault() });
	    }

		//
        [ActionName("modal-envio-cadastro-por-email")]
        public ActionResult modalEnvioCadastroPorEmail(int idAssociado) {

            var ViewModel = new AssociadoEnvioCadastroEmailForm();

            ViewModel.idAssociado = idAssociado;

            return View(ViewModel);

        }

		//
		[HttpPost, ActionName("enviar-cadastro-por-email")]
        public ActionResult enviarCadastroPorEmail(AssociadoEnvioCadastroEmailForm ViewModel) {

            if (!ModelState.IsValid) {

                return View("modal-envio-cadastro-por-email", ViewModel);

            }

            var Retorno = this.OAssociadoEnvioFichaBL.enviarPorEmail(ViewModel.idAssociado, ViewModel.emailsDestino, User.id());

            if (!Retorno.flagError) {

                ViewModel.emailsDestino = "";

                this.Flash(UtilMessage.TYPE_MESSAGE_SUCCESS, "A ficha de cadastro do associado foi enviada com sucesso.");

                return Json(Retorno, JsonRequestBehavior.AllowGet);
            }
            
            return View("modal-envio-cadastro-por-email", ViewModel);
        }

	}
}
