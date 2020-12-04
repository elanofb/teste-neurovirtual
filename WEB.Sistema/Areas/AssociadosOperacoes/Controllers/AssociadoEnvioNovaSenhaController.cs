using System;
using System.Linq;
using System.Web.Mvc;
using BLL.Associados;
using BLL.AssociadosOperacoes;
using DAL.Associados.DTO;
using MvcFlashMessages;
using WEB.Areas.AssociadosOperacoes.ViewModels;

namespace WEB.Areas.AssociadosOperacoes.Controllers {

	public class AssociadoEnvioNovaSenhaController : Controller {

		//Atributos
        private IAssociadoConsultaBL _IAssociadoBL;
		private IAssociadoNovaSenhaNotificacaoBL _IAssociadoNovaSenhaNotificacaoBL;

		//Propriedades
        private IAssociadoConsultaBL OAssociadoBL => _IAssociadoBL = _IAssociadoBL ?? new AssociadoConsultaBL();
		private IAssociadoNovaSenhaNotificacaoBL OAssociadoNovaSenhaNotificacaoBL => _IAssociadoNovaSenhaNotificacaoBL = _IAssociadoNovaSenhaNotificacaoBL ?? new AssociadoNovaSenhaNotificacaoBL();
        
	    //Abertura do modal para configurar a exclusão dos associados
		[HttpPost, ActionName("modal-enviar-nova-senha")]
		public ActionResult modalEnviarNovaSenha(AssociadoFiltroVM DadosConsulta) {
            
			var ViewModel = new AssociadoEnvioNovaSenhaForm();
            
            ViewModel.listaAssociados = DadosConsulta.montarQuery()
                                                     .Select(x => new ItemListaAssociado {
                                                         id = x.id, nome = x.nome,
                                                         nroAssociado = x.nroAssociado,
                                                         descricaoTipoAssociado = x.descricaoTipoAssociado,
                                                         idPessoa = x.idPessoa, ativo = x.ativo
                                                     }).OrderBy(x => x.nome).ToList();

		    if (!ViewModel.listaAssociados.Any()) {

		        return Json(new { error = true, message = "Nenhum associado foi encontrado enviar uma nova senha." }, JsonRequestBehavior.AllowGet);

		    }

		    ViewModel.idsAssociados = ViewModel.listaAssociados.Select(x => x.id).ToList();

			return PartialView(ViewModel);

		}

        //
        [HttpPost, ActionName("enviar-senha")]
        public ActionResult enviarSenha(AssociadoEnvioNovaSenhaForm ViewModel) {

            ViewModel.listaAssociados = this.OAssociadoBL.queryNoFilter(1)
                                            .Where(x => ViewModel.idsAssociados.Contains(x.id))
                                            .Select(x => new ItemListaAssociado {
                                                id = x.id, 
												 idPessoa = x.idPessoa, 
												 nome = x.Pessoa.nome,
                                                nroAssociado = x.nroAssociado, descricaoTipoAssociado = x.TipoAssociado.nomeDisplay, ativo = x.ativo,
                                                email = x.Pessoa.listaEmails.Where(c => !c.email.Equals("") && c.dtExclusao == null).OrderByDescending(c => c.id).FirstOrDefault().email,
                                                emailSecundario = x.Pessoa.listaEmails.Where(c => !c.email.Equals("") && c.dtExclusao == null).OrderByDescending(c => c.id).Skip(1).FirstOrDefault().email
                                            }).OrderBy(x => x.nome).ToList();

            if (!ModelState.IsValid) {

                return View("modal-enviar-nova-senha", ViewModel);    

            }

            var ORetorno = this.OAssociadoNovaSenhaNotificacaoBL.registrarNovaSenha(ViewModel.listaAssociados, ViewModel.novaSenha);

            if (!ORetorno.flagError) {

                this.Flash(UtilMessage.TYPE_MESSAGE_SUCCESS, UtilMessage.success("Sucesso!", ORetorno.listaErros.FirstOrDefault()));

                return Json(new { error = false }, JsonRequestBehavior.AllowGet);

            }

            return Json(new { error = true, message = ORetorno.listaErros.FirstOrDefault() }, JsonRequestBehavior.AllowGet);

        }

        //
        [ActionName("gerar-senha-aleatoria")]
	    public ActionResult gerarSenhaAleatoria(AssociadoEnvioNovaSenhaForm ViewModel) {

            var senhaGerada = UtilString.randomString(6);

            return Json(new { senhaGerada }, JsonRequestBehavior.AllowGet);

        }

	}
}
