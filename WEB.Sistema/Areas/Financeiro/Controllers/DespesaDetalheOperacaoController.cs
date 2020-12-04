using System;
using System.Linq;
using System.Web.Mvc;
using DAL.Financeiro;
using BLL.Financeiro;
using BLL.LogsAlteracoes;
using MvcFlashMessages;

namespace WEB.Areas.Financeiro.Controllers {

    [OrganizacaoFilter]
    public class DespesaDetalheOperacaoController : Controller {

        //Atributos
        private ITituloDespesaBL _ContasAPagarBL;
        private ILogTituloDespesaBL _LogTituloDespesaBL;

        //Propriedades
        private ITituloDespesaBL OTituloDespesaBL => _ContasAPagarBL = _ContasAPagarBL ?? new TituloDespesaPadraoBL();
        private ILogTituloDespesaBL OLogTituloDespesaBL => _LogTituloDespesaBL = _LogTituloDespesaBL ?? new LogTituloDespesaBL();

        //Recebe as alterações do titulo de cobrança
        //Os valores são enviados individualmente
        [HttpPost, ActionName("alterar-dados")]
        public ActionResult alterarDados() {

            var id = UtilRequest.getInt32("pk");
            var nomeCampo = UtilRequest.getString("name").Trim();
            var valorCampo = UtilRequest.getString("value").Trim();
            var nomeCampoDisplay = UtilRequest.getString("nomeCampoDisplay").Trim();
            var oldValue = UtilRequest.getString("oldValue").Trim();
            var newValue = UtilRequest.getString("newValue").Trim();

            var ORetorno = OLogTituloDespesaBL.alterarCampo(id, nomeCampo, valorCampo, "", nomeCampoDisplay, oldValue, newValue);

            return Json(new { error = ORetorno.flagError, message = string.Join("<br/>", ORetorno.listaErros.ToArray()) });
        }

        [HttpGet, ActionName("modal-excluir-despesa")]
        public ActionResult modalExcluirDespesa(int? id) {

            var OTituloDespesa = this.OTituloDespesaBL.carregar(id.toInt());

            if (OTituloDespesa == null) {
                return Json(new {flagErro = true, message = "A receita não pode ser localizada."}, JsonRequestBehavior.AllowGet);
            }

            return View(OTituloDespesa);
        }


        [HttpPost]
        public ActionResult excluir(TituloDespesa ViewModel) {

            if (ViewModel.motivoExclusao.isEmpty()) {
                this.Flash(UtilMessage.TYPE_MESSAGE_ERROR, "Informe o motivo da exclusão.");
                return View("modal-excluir-despesa", ViewModel);
            }
            
            var ORetorno = this.OTituloDespesaBL.excluir(ViewModel.id, ViewModel.motivoExclusao);

            if (ORetorno.flagError == false) {
                return Json(new {error = false, message = "Despesa removida com sucesso", urlRetorno = Url.Action("listar", "LancamentoDespesas", new {area = "FinanceiroLancamentos"})});
            }

            this.Flash(UtilMessage.TYPE_MESSAGE_ERROR, ORetorno.listaErros.FirstOrDefault());
            return View("modal-excluir-despesa", ViewModel);
        }
    }
}
