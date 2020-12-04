using System;
using System.Web.Mvc;
using BLL.AssociadosContribuicoes;
using DAL.AssociadosContribuicoes;
using DAL.Permissao.Security.Extensions;
using MvcFlashMessages;
using WEB.App_Infrastructure;

namespace WEB.Areas.AssociadosContribuicoes.Controllers{

    public class AssociadoContribuicaoIsencaoController : BaseSistemaController{

		//Atributos
		private IAssociadoContribuicaoBL _AssociadoContribuicaoBL; 
		private IAssociadoContribuicaoIsencaoBL _AssociadoContribuicaoIsencaoBL; 

		//Propriedades
        private IAssociadoContribuicaoBL OAssociadoContribuicaoBL => this._AssociadoContribuicaoBL = this._AssociadoContribuicaoBL ?? new AssociadoContribuicaoBL();
        private IAssociadoContribuicaoIsencaoBL OAssociadoContribuicaoIsencaoBL => this._AssociadoContribuicaoIsencaoBL = this._AssociadoContribuicaoIsencaoBL ?? new AssociadoContribuicaoIsencaoBL();


		//Abertura do modal para configurar a inselçao da anuidade
		[ActionName("partial-isencao-contribuicao"), HttpGet]
		public PartialViewResult partialIsencaoContribuicao(int id) {
            
			AssociadoContribuicao OAssociadoContribuicao = this.OAssociadoContribuicaoBL.carregar(id);

			return PartialView(OAssociadoContribuicao);
		}

        //
		[ActionName("conceder-isencao"), HttpPost]
		public ActionResult concederIsencao(AssociadoContribuicao ViewModel, string observacoes) {
            
			var OAssociadoContribuicao = this.OAssociadoContribuicaoBL.carregar(ViewModel.id);

			if (OAssociadoContribuicao == null) { 
				ModelState.AddModelError("", "A cobrança não pôde ser localizado.");
				return PartialView("partial-isencao-contribuicao", ViewModel);
			}

			if (String.IsNullOrEmpty(observacoes)) { 
				ModelState.AddModelError("observacoes", "Informe o motivo para a isenção.");
				return PartialView("partial-isencao-contribuicao", ViewModel);
			}

            if (observacoes.Length > 100) { 
				ModelState.AddModelError("observacoes", "O motivo para a isenção não pode ultrapassar 100 caracteres.");
				return PartialView("partial-isencao-contribuicao", ViewModel);
			}

			this.OAssociadoContribuicaoIsencaoBL.concederIsencao(ViewModel.id, observacoes, User.id());

            this.Flash(UtilMessage.TYPE_MESSAGE_SUCCESS, "A isenção da cobrança foi concedida com sucesso!");

            return Json( new{ flagSucesso = true} );
		}
    }
}
