using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using BLL.AssociadosNotificacoes.Services;
using BLL.Configuracoes;
using BLL.Contribuicoes;
using DAL.Configuracoes;
using MvcFlashMessages;
using WEB.App_Infrastructure;
using WEB.Areas.AssociadosNotificacoes.ViewModels;
using WEB.Areas.ContribuicoesPainel.ViewModels;

namespace WEB.Areas.AssociadosNotificacoes.Controllers{

    public class AssociadoContribuicaoCobrancaController : BaseSistemaController{

		//Atributos
		private IContribuicaoBL _IContribuicaoBL; 
        private IAssociadoContribuicaoCobrancaNotificacaoBL _IAssociadoContribuicaoCobrancaNotificacaoBL;

		//Propriedades
        private IContribuicaoBL OContribuicaoBL => this._IContribuicaoBL = this._IContribuicaoBL ?? new ContribuicaoPadraoBL();
        private IAssociadoContribuicaoCobrancaNotificacaoBL OAssociadoContribuicaoCobrancaNotificacaoBL => this._IAssociadoContribuicaoCobrancaNotificacaoBL = this._IAssociadoContribuicaoCobrancaNotificacaoBL ?? new AssociadoContribuicaoCobrancaNotificacaoBL();
        private ConfiguracaoSistema OConfiguracaoSistema => ConfiguracaoSistemaBL.getInstance.carregar();

        //
        [HttpPost, ActionName("modal-gerar-email-cobranca")]
		public ActionResult modalEnviarCobranca(int idContribuicao, List<int> idsAssociadoContribuicoes) {

            var ViewModel = new AssociadoContribuicaoCobrancaForm();

            ViewModel.Contribuicao = this.OContribuicaoBL.carregar(idContribuicao);
            
            ViewModel.Contribuicao.emailCobrancaTitulo = ViewModel.Contribuicao.emailCobrancaTitulo.Replace("#NOME_ORGANIZACAO#", this.OConfiguracaoSistema.nomeEmpresaResumo);

            ViewModel.idsAssociadoContribuicoes = idsAssociadoContribuicoes;

            return View(ViewModel);

        }

        //
        [HttpPost, ActionName("modal-gerar-email-cobranca-todos")]
        public ActionResult modalEnviarCobrancaTodos(int idContribuicao) {
            
            var OPainelCobrancaVM = new PainelCobrancaVM();

            OPainelCobrancaVM.carregarDadosContribuicao(idContribuicao, null);

            var ViewModel = new AssociadoContribuicaoCobrancaForm();

            ViewModel.Contribuicao = OPainelCobrancaVM.Contribuicao;

            ViewModel.Contribuicao.emailCobrancaTitulo = ViewModel.Contribuicao.emailCobrancaTitulo.Replace("#NOME_ORGANIZACAO#", this.OConfiguracaoSistema.nomeEmpresaResumo);

            var listaPendentes = OPainelCobrancaVM.listaCobrancas.Where(x => !x.AssociadoContribuicao.flagQuitado()).ToList();

            ViewModel.idsAssociadoContribuicoes = listaPendentes.Select(x => x.AssociadoContribuicao.id).ToList();

            return View("modal-gerar-email-cobranca", ViewModel);

        }

        [HttpPost, ActionName("gerar-email-cobranca"), ValidateInput(false)]
        public ActionResult gerarEmailCobranca(AssociadoContribuicaoCobrancaForm ViewModel) {

            if (!ModelState.IsValid) {
                return View("modal-gerar-email-cobranca", ViewModel);
            }

            var flagSucesso = this.OAssociadoContribuicaoCobrancaNotificacaoBL.registrarEmailsCobrancas(ViewModel.Contribuicao, ViewModel.idsAssociadoContribuicoes);

            if (flagSucesso) {
                
                return Json(new { error = false, message = "Os emails de cobrança foram gerados com sucesso." }, JsonRequestBehavior.AllowGet);

            }

            this.Flash(UtilMessage.TYPE_MESSAGE_ERROR, UtilMessage.error("Erro!", "Houve algum problema ao gerar os emails de cobranças."));

            return View("modal-gerar-email-cobranca", ViewModel);

        }

    }
}
