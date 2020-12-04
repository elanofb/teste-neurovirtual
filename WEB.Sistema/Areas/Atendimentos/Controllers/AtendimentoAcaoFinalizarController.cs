using System;
using System.Web.Mvc;
using BLL.Atendimentos;
using BLL.Transacoes.Compras;
using BLL.Transacoes.Debitos;
using DAL.Atendimentos;
using MvcFlashMessages;
using WEB.Areas.Atendimentos.ViewModels;

namespace WEB.Areas.Atendimentos.Controllers {

	public class AtendimentoAcaoFinalizarController : Controller {

        //Atributos
        private IAtendimentoAcaoBL _IAtendimentoAcaoBL;
	    private IAtendimentoConsultaBL _IAtendimentoConsultaBL;
	    private IMediadorBase _MediadorSaque;
	    private IDebitoFacade _DebitoFacade;
        
		//Propriedades
		private IAtendimentoAcaoBL OAtendimentoAcaoBL => _IAtendimentoAcaoBL = _IAtendimentoAcaoBL ?? new AtendimentoAcaoBL();
	    private IAtendimentoConsultaBL OAtendimentoConsultaBL => _IAtendimentoConsultaBL = _IAtendimentoConsultaBL ?? new AtendimentoConsultaBL();	    
	    private IMediadorBase MediadorSaque => _MediadorSaque = _MediadorSaque ?? new MediadorSaque();
	    private IDebitoFacade DebitoFacade => _DebitoFacade = _DebitoFacade ?? new DebitoFacade();
	    

        //
        [ActionName("modal-finalizar")]
        public ActionResult modalFinalizar(int id) {

            var ViewModel = new AtendimentoAcaoFinalizarForm();

            ViewModel.AtendimentoHistorico.idAtendimento = id;

            ViewModel.AtendimentoHistorico.flagAtendido = true;

            return View(ViewModel);

        }

        [HttpPost, ActionName("finalizar")]
        public ActionResult finalizar(AtendimentoAcaoFinalizarForm ViewModel) {
			
            if (!ModelState.IsValid) {

                return View("modal-finalizar", ViewModel);

            }
			
            if (ViewModel.AtendimentoHistorico.flagAtendido == true) { 
				
                ViewModel.AtendimentoHistorico.mensagem = "";
	            
	            /*Atendimento OAtendimento = this.OAtendimentoConsultaBL.carregar(ViewModel.AtendimentoHistorico.idAtendimento);
                
	            if (OAtendimento.idTipoAtendimento == TipoAtendimentoConst.SAQUE && OAtendimento.valor > 0){
                    
		            var Movimento = this.MediadorSaque.carregarDados(0, OAtendimento.idPessoa.toInt(), OAtendimento.valor.Value, 0);		           
                    
		            UtilRetorno ORetorno = this.DebitoFacade.debitar(Movimento);
                    
		            if (ORetorno.flagError){
                        
			            string error = string.Join("<br>", ORetorno.listaErros); 
                        
			            this.Flash(UtilMessage.TYPE_MESSAGE_ERROR, error);
                        
			            return View("modal-finalizar", ViewModel);                  
                        
		            }
                    
	            }*/

            }
				
            this.OAtendimentoAcaoBL.finalizar(ViewModel.AtendimentoHistorico);
	                    
            return Json(new { error = false, message = "O atendimento foi finalizado com sucesso." }, JsonRequestBehavior.AllowGet);

        }


    }
}