using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using BLL.Associados;
using BLL.AvisosNotificacoes.Services;
using DAL.Associados.DTO;
using DAL.Notificacoes;
using MvcFlashMessages;
using WEB.Areas.AssociadosNotificacoes.ViewModels;

namespace WEB.Areas.AssociadosNotificacoes.Controllers {

	public class AssociadoNotificacaoController : Controller {

		//Atributos
        private IAssociadoBL _IAssociadoBL;
	    private INotificacaoAssociadoAvulsaBL _INotificacaoAssociadoAvulsaBL;

		//Propriedades
        private IAssociadoBL OAssociadoBL => _IAssociadoBL = _IAssociadoBL ?? new AssociadoBL();
	    private INotificacaoAssociadoAvulsaBL ONotificacaoAssociadoAvulsaBL => _INotificacaoAssociadoAvulsaBL = _INotificacaoAssociadoAvulsaBL ?? new NotificacaoAssociadoAvulsaBL();
		
        
	    //Abertura do modal para configurar a notificação
		[HttpPost, ActionName("modal-notificar-associados")]
		public ActionResult modalNotificarAssociados(AssociadoNotificacaoVM DadosConsulta) {
            
			var ViewModel = new AssociadoNotificacaoForm();

            ViewModel.ONotificacao = new NotificacaoSistema() { dtProgramacaoEnvio = DateTime.Today };
            
            ViewModel.listaAssociados = DadosConsulta.montarQuery()
                                                     .Select(x => new ItemListaAssociado {

                                                         id = x.id, nome = x.nome,
                                                         nroAssociado = x.nroAssociado,
                                                         descricaoTipoAssociado = x.descricaoTipoAssociado,
                                                         idPessoa = x.idPessoa

                                                     }).OrderBy(x => x.nome).ToList();

		    if (!ViewModel.listaAssociados.Any()) {

		        return Json(new { error = true, message = "Nenhum associado foi encontrado para enviar mensagem." }, JsonRequestBehavior.AllowGet);

		    }

		    ViewModel.idsAssociados = ViewModel.listaAssociados.Select(x => x.id).ToList();

			return PartialView(ViewModel);

		}

        [HttpPost, ActionName("notificar-associados"), ValidateInput(false)]
        public ActionResult notificarAssociados(AssociadoNotificacaoForm ViewModel) {

            ViewModel.listaAssociados = this.OAssociadoBL.listar(0, "", "", "")
                                            .Where(x => ViewModel.idsAssociados.Contains(x.id))
                                            .Select(x => new ItemListaAssociado {

                                                id = x.id, nome = x.Pessoa.nome, nroAssociado = x.nroAssociado,
                                                descricaoTipoAssociado = x.TipoAssociado.nomeDisplay, idPessoa = x.idPessoa,
                                                email = x.Pessoa.listaEmails.Where(c => !c.email.Equals("") && c.dtExclusao == null).OrderByDescending(c => c.id).FirstOrDefault().email,
                                                emailSecundario = x.Pessoa.listaEmails.Where(c => !c.email.Equals("") && c.dtExclusao == null).OrderByDescending(c => c.id).Skip(1).FirstOrDefault().email

                                            }).OrderBy(x => x.nome).ToList();

            if (!ModelState.IsValid) {

                return View("modal-notificar-associados", ViewModel);    
            }

            ViewModel.ONotificacao.flagAssociadosEspecificos = true;
            
            var listaAssociadosEnvio = new List<NotificacaoSistemaEnvio>();

            foreach (var OAssociado in ViewModel.listaAssociados) {

	            var listaEmails = new List<string> { OAssociado.email, OAssociado.emailSecundario };

	            foreach (var email in listaEmails) {
		            
		            var OEnvio = new NotificacaoSistemaEnvio();

		            OEnvio.idReferencia = OAssociado.id;
                
		            OEnvio.nome = OAssociado.nome;

		            OEnvio.email = email;
		            
		            if (!UtilValidation.isEmail(OEnvio.email)) {

			            OEnvio.flagExcluido = true;

			            OEnvio.motivoExclusao = "O e-mail configurado não é válido.";
		            }

		            listaAssociadosEnvio.Add(OEnvio);
		            
	            }
	            
            }
            
            if (!listaAssociadosEnvio.Any()) {
                return Json(new { error = true, message = "Nenhum associado foi informado para enviar a mensagem. Tente novamente." }, JsonRequestBehavior.AllowGet);
            }

            var flagSucesso = this.ONotificacaoAssociadoAvulsaBL.salvar(ViewModel.ONotificacao, listaAssociadosEnvio);

            if (flagSucesso) {

                this.Flash(UtilMessage.TYPE_MESSAGE_SUCCESS, UtilMessage.success("Sucesso!", "Mensagem registrada com sucesso. Os associados a receberão na data informada."));

                return Json(new { error = false }, JsonRequestBehavior.AllowGet);

            }

            return Json(new { error = true, message = "Houve algum problema ao enviar a mensagem para os associados. Tente novamente." }, JsonRequestBehavior.AllowGet);

        }

        
	}
}
