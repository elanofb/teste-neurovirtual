using System;
using System.Web.Mvc;
using DAL.Transacoes;
using MvcFlashMessages;
using WEB.App_Infrastructure;
using WEB.Areas.Transacoes.ViewModels;

namespace WEB.Areas.Transacoes.Controllers {

    public class LancamentoDebitoController : BaseSistemaController{
        
        //  
        public ActionResult index() {
            
            DebitoVM ViewModel = new DebitoVM();
                
            ViewModel.carregarParametros();

            var tipoOperacao = UtilRequest.getString("tipoOperacao");
            var nomeMembroDestino = UtilRequest.getString("nomeMembroDestino");

            if (tipoOperacao == MovimentoConst.VALIDAR) {

                var ORetorno = ViewModel.validar();

                if (ORetorno.flagError) {
                    this.Flash(UtilMessage.TYPE_MESSAGE_ERROR,
                        UtilMessage.errorFaceDown("Falha!", string.Join("<br/>", ORetorno.listaErros)));
                    return View(ViewModel);
                }
                
                return View(ViewModel);
            }
            
            if(tipoOperacao == MovimentoConst.EFETIVAR){
                
                ViewModel.OMovimentoOperacaoDTO.idTipoTransacao = (byte) TipoTransacaoEnum.LANCAMENTO_DEBITO;                
                                                
                var ORetorno = ViewModel.debitar();

                if (ORetorno.flagError) {
                    this.Flash(UtilMessage.TYPE_MESSAGE_ERROR,
                        UtilMessage.errorFaceDown("Falha!", string.Join("<br/>", ORetorno.listaErros)));
                    return View(ViewModel);
                }

                var textoSucesso = "O lançamento de débito foi realizado:" +
                                   "<br/>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<strong>Destino:</strong> " + nomeMembroDestino +
                                   "<br/>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<strong>Movimento:</strong> #" + ViewModel.OMovimento.id;
                
                this.Flash(UtilMessage.TYPE_MESSAGE_SUCCESS, UtilMessage.success("Concluído!", textoSucesso));
                
                return View(new DebitoVM());
                
            }
            
            return View(ViewModel);            
        }     
    
    }

}
