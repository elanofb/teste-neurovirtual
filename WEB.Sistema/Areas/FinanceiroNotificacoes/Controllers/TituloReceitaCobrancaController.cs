using System;
using System.Collections.Generic;
using System.Web.Mvc;
using BLL.Configuracoes;
using DAL.Configuracoes;
using MvcFlashMessages;
using WEB.App_Infrastructure;
using WEB.Areas.FinanceiroNotificacoes.ViewModels;

namespace WEB.Areas.FinanceiroNotificacoes.Controllers{

    public class TituloReceitaCobrancaController : BaseSistemaController{

		//Atributos

        //Propriedades
        private ConfiguracaoNotificacao OConfigNotificacao => ConfiguracaoNotificacaoBL.getInstance.carregar();
        private ConfiguracaoSistema OConfigSistema => ConfiguracaoSistemaBL.getInstance.carregar();

        //
        [HttpPost, ActionName("modal-gerar-email-cobranca")]
		public ActionResult modalEnviarCobranca(List<int> idsTitulosReceita) {

            var ViewModel = new TituloReceitaCobrancaForm();

            ViewModel.emailCobrancaTitulo = this.OConfigNotificacao.tituloEmailCobranca.stringOrEmpty();

            ViewModel.emailCobrancaTitulo = ViewModel.emailCobrancaTitulo.Replace("#NOME_ORGANIZACAO#", this.OConfigSistema.nomeEmpresaResumo);

            ViewModel.emailCobrancaHtml = this.OConfigNotificacao.corpoEmailCobranca;

            ViewModel.idsTitulosReceita = idsTitulosReceita;
            
            return View(ViewModel);

        }
        
        [HttpPost, ActionName("gerar-email-cobranca"), ValidateInput(false)]
        public ActionResult gerarEmailCobranca(TituloReceitaCobrancaForm ViewModel) {

            if (!ModelState.IsValid) {
                return View("modal-gerar-email-cobranca", ViewModel);
            }

            var flagSucesso = true; // this.OTituloReceitaCobrancaNotificacaoBL.registrarEmailsCobrancas(ViewModel.idsTitulosReceita, ViewModel.emailCobrancaTitulo, ViewModel.emailCobrancaHtml);

            if (flagSucesso) {
                
                return Json(new { error = false, message = "Os emails de cobrança foram gerados com sucesso." }, JsonRequestBehavior.AllowGet);

            }

            this.Flash(UtilMessage.TYPE_MESSAGE_ERROR, UtilMessage.error("Erro!", "Houve algum problema ao gerar os emails de cobranças."));

            return View("modal-gerar-email-cobranca", ViewModel);

        }

    }
}
