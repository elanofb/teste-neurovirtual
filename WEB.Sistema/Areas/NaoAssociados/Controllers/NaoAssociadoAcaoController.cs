using System;
using System.Linq;
using System.Web.Mvc;
using DAL.Associados;
using System.Json;
using BLL.NaoAssociados;
using DAL.Permissao.Security.Extensions;
using MvcFlashMessages;
using WEB.Areas.NaoAssociados.ViewModels;

namespace WEB.Areas.NaoAssociados.Controllers {

	public class NaoAssociadoAcaoController : Controller {

		//Atributos
		private INaoAssociadoBL _NaoAssociadoBL;
		private INaoAssociadoAcaoBL _NaoAssociadoAcaoBL;
        private INaoAssociadoEnvioFichaBL _NaONaoAssociadoEnvioFichaBL;

		//Propriedades
		private INaoAssociadoBL ONaoAssociadoBL => (this._NaoAssociadoBL = this._NaoAssociadoBL ?? new NaoAssociadoBL());
	    private INaoAssociadoAcaoBL ONaoAssociadoAcaoBL => (this._NaoAssociadoAcaoBL = this._NaoAssociadoAcaoBL ?? new NaoAssociadoAcaoBL());
        private INaoAssociadoEnvioFichaBL ONaoAssociadoEnvioFichaBL => (this._NaONaoAssociadoEnvioFichaBL = this._NaONaoAssociadoEnvioFichaBL ?? new NaoAssociadoEnvioFichaBL());

	    //
		[ActionName("partial-desativacao-nao-associado"), HttpGet]
		public PartialViewResult partialDesativacaoAssociado(int idAssociado) {
            
			Associado OAssociado = this.ONaoAssociadoBL.carregar(idAssociado).condicoesSeguranca().FirstOrDefault();

			return PartialView(OAssociado);
		}


		//
		[ActionName("desativar-nao-associado"), HttpPost]
		public ActionResult desativarAssociado(Associado ViewModel, string observacoes) {
            
			Associado OAssociado = this.ONaoAssociadoBL.carregar(ViewModel.id).condicoesSeguranca().FirstOrDefault();

			if (OAssociado == null) { 
				ModelState.AddModelError("", "O cadastro não pôde ser localizado.");
				return PartialView("partial-desativacao-nao-associado", ViewModel);
			}

			if (OAssociado.ativo == "N") { 
				ModelState.AddModelError("", "Esse nãO cadastro já está desativado.");
				return PartialView("partial-desativacao-nao-associado", ViewModel);
			}

			if (String.IsNullOrEmpty(observacoes)) { 
				ModelState.AddModelError("observacoes", "Informe o motivo para desativação.");
				return PartialView("partial-desativacao-nao-associado", ViewModel);
			}

			this.ONaoAssociadoAcaoBL.desativarNaoAssociado(ViewModel.id, observacoes);
            this.Flash(UtilMessage.TYPE_MESSAGE_SUCCESS, "O cadastro foi desativado com sucesso!");
			return Json( new{ flagSucesso = true} );
		}

		//
		[ActionName("partial-reativacao-nao-associado"), HttpGet]
		public PartialViewResult partialReativacaoAssociado(int idAssociado) {
            
			Associado OAssociado = this.ONaoAssociadoBL.carregar(idAssociado).condicoesSeguranca().FirstOrDefault();

			return PartialView(OAssociado);
		}

		//
		[ActionName("reativar-nao-associado"), HttpPost]
		public ActionResult reativarAssociado(Associado ViewModel, string observacoes) {
            
			Associado OAssociado = this.ONaoAssociadoBL.carregar(ViewModel.id).condicoesSeguranca().FirstOrDefault();

			if (OAssociado == null) { 
				ModelState.AddModelError("", "O cadastro não pôde ser localizado.");
				return PartialView("partial-reativacao-nao-associado", ViewModel);
			}

			if (OAssociado.ativo != "N") { 
				ModelState.AddModelError("", "Esse associado nãoestá desativado.");
				return PartialView("partial-reativacao-nao-associado", ViewModel);
			}

			if (String.IsNullOrEmpty(observacoes)) { 
				ModelState.AddModelError("observacoes", "Informe o motivo para reativação.");
				return PartialView("partial-reativacao-nao-associado", ViewModel);
			}

			this.ONaoAssociadoAcaoBL.reativarNaoAssociado(ViewModel.id, observacoes);
            this.Flash(UtilMessage.TYPE_MESSAGE_SUCCESS, "O cadastro foi reativado com sucesso!");
			return Json( new{ flagSucesso = true} );
		}

		//
		[HttpPost, ActionName("reenviar-senha")]
		public ActionResult reenviarSenha(int idAssociado) {
			
			var Retorno = this.ONaoAssociadoAcaoBL.enviarLinkSenha(idAssociado);

			return Json(new JsonMessage{ error = Retorno.flagError, message = Retorno.listaErros.FirstOrDefault() } );
		}

		//
		[ActionName("partial-excluir-nao-associado"), HttpGet]
		public PartialViewResult partialExcluirAssociado(int idAssociado) {
            
			Associado OAssociado = this.ONaoAssociadoBL.carregar(idAssociado).condicoesSeguranca().FirstOrDefault();

			return PartialView(OAssociado);
		}


		//
		[ActionName("excluir-nao-associado"), HttpPost]
		public ActionResult excluirAssociado(Associado ViewModel, string observacoes) {
            
			Associado OAssociado = this.ONaoAssociadoBL.carregar(ViewModel.id).condicoesSeguranca().FirstOrDefault();

			if (OAssociado == null) { 
				ModelState.AddModelError("", "O cadastro não pôde ser localizado.");
				return PartialView("partial-desativacao-nao-associado", ViewModel);
			}

			if (String.IsNullOrEmpty(observacoes)) { 
				ModelState.AddModelError("observacoes", "Informe o motivo para a exclusão.");
				return PartialView("partial-excluir-nao-associado", ViewModel);
			}

            if (observacoes.Length > 100) { 
				ModelState.AddModelError("observacoes", "O motivo para a exclusão não pode ultrapassar 100 caracteres.");
				return PartialView("partial-excluir-nao-associado", ViewModel);
			}

			this.ONaoAssociadoAcaoBL.excluirNaoAssociado(ViewModel.id, observacoes);
            this.Flash(UtilMessage.TYPE_MESSAGE_SUCCESS, "O cadastro foi excluído com sucesso!");
			return Json( new{ flagSucesso = true} );
		}


		//
		[ActionName("partial-tornar-associado"), HttpGet]
		public PartialViewResult partialTornarAssociado(int idAssociado) {

		    var Model = new TornarAssociadoForm();
			Model.NaoAssociado = this.ONaoAssociadoBL.carregar(idAssociado).condicoesSeguranca().FirstOrDefault();

			return PartialView(Model);
		}

		//
		[ActionName("tornar-associado"), HttpPost]
		public ActionResult tornarAssociado(TornarAssociadoForm ViewModel) {
            
			Associado OAssociado = this.ONaoAssociadoBL.carregar(ViewModel.NaoAssociado.id).condicoesSeguranca().FirstOrDefault();

			if (OAssociado == null) { 
				ModelState.AddModelError("", "O cadastro não pôde ser localizado.");
				return PartialView("partial-tornar-associado", ViewModel);
			}

			if (ViewModel.flagDesejaAdmitir == "S" && ViewModel.NaoAssociado.dtAdmissao == null) { 
				ModelState.AddModelError("", "Informe a data de admissão.");
				return PartialView("partial-tornar-associado", ViewModel);
			}

			if (ViewModel.NaoAssociado.idTipoAssociado == 0) { 
				ModelState.AddModelError("", "Informe o tipo de associado.");
				return PartialView("partial-tornar-associado", ViewModel);
			}

			if (ViewModel.NaoAssociado.idTipoAssociado == TipoAssociadoConst.NAO_ASSOCIADO) { 
				ModelState.AddModelError("", "Não é possível escolher o tipo 'Não Associado'.");
				return PartialView("partial-tornar-associado", ViewModel);
			}

            if (ViewModel.observacoes != null && ViewModel.observacoes.Length > 100) { 
				ModelState.AddModelError("observacoes", "A observação não pode ultrapassar 100 caracteres.");
				return PartialView("partial-tornar-associado", ViewModel);
			}

		    var Associado = new Associado();
		    Associado.id = ViewModel.NaoAssociado.id;
		    Associado.idTipoAssociado = Convert.ToInt32(ViewModel.NaoAssociado.idTipoAssociado);
		    Associado.ativo = ViewModel.flagDesejaAdmitir == "S" ? "S" : "E";
		    Associado.dtAdmissao = ViewModel.flagDesejaAdmitir == "S" ? ViewModel.NaoAssociado.dtAdmissao : null;

		    this.ONaoAssociadoAcaoBL.tornarAssociado(Associado, ViewModel.observacoes);

            this.Flash(UtilMessage.TYPE_MESSAGE_SUCCESS, "Tornou-se associado com sucesso!");

            return Json( new{ flagSucesso = true, ViewModel.NaoAssociado.id } );
		}

        //
        [ActionName("alterar-tipo-nao-associado"), HttpPost]
	    public ActionResult alterarTipoAssociado(int idAssociado, int idTipoAssociado) {

	        var Retorno = this.ONaoAssociadoAcaoBL.alterarTipo(idAssociado, idTipoAssociado, User.id());

            if (!Retorno.flagError) {
                this.Flash(UtilMessage.TYPE_MESSAGE_SUCCESS, "<strong>Sucesso!</strong><br />O tipo de não associado foi alterado com sucesso.");
            }

	        return Json(Retorno, JsonRequestBehavior.AllowGet);
	    }

        //
        [ActionName("modal-envio-cadastro-por-email")]
        public ActionResult modalEnvioCadastroPorEmail(int idAssociado) {

            var ViewModel = new NaoAssociadoEnvioCadastroEmailForm();
            ViewModel.idAssociado = idAssociado;

            return View(ViewModel);

        }

		//
		[HttpPost, ActionName("enviar-cadastro-por-email")]
        public ActionResult enviarCadastroPorEmail(NaoAssociadoEnvioCadastroEmailForm ViewModel) {

            if (!ModelState.IsValid) {
                return View("modal-envio-cadastro-por-email", ViewModel);
            }

            var Retorno = this.ONaoAssociadoEnvioFichaBL.enviarPorEmail(ViewModel.idAssociado, ViewModel.emailsDestino, User.id());

            if (!Retorno.flagError) {
                ViewModel.emailsDestino = "";

                this.Flash(UtilMessage.TYPE_MESSAGE_SUCCESS, "A ficha de cadastro do não associado foi enviada com sucesso.");
                return Json(Retorno, JsonRequestBehavior.AllowGet);
            }
            
            return View("modal-envio-cadastro-por-email", ViewModel);
        }
	}
}
