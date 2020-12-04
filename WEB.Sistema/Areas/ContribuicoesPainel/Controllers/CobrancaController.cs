using System;
using System.Linq;
using System.Web.Mvc;
using BLL.AssociadosContribuicoes;
using WEB.Areas.ContribuicoesPainel.ViewModels;

namespace WEB.Areas.ContribuicoesPainel.Controllers {

    [OrganizacaoFilter]
    public class CobrancaController : Controller {

        //Atributos
        private IAssociadoContribuicaoFilaGeracaoBL _AssociadoContribuicaoFilaGeracaoBL;
        private IAssociadoContribuicaoBL _AssociadoContribuicaoBL;

        //Propriedades
        private IAssociadoContribuicaoFilaGeracaoBL OAssociadoContribuicaoFilaGeracaoBL => _AssociadoContribuicaoFilaGeracaoBL = _AssociadoContribuicaoFilaGeracaoBL ?? new AssociadoContribuicaoFilaGeracaoBL();
        private IAssociadoContribuicaoBL OAssociadoContribuicaoBL => _AssociadoContribuicaoBL = _AssociadoContribuicaoBL ?? new AssociadoContribuicaoBL();

        /// <summary>
        /// Gerar as cobranças
        /// </summary>
        [ActionName("gerar-cobrancas")]
        public ActionResult gerarCobrancas() {

            int idContribuicao = UtilRequest.getInt32("idContribuicao");

            int[] idsAssociados = UtilRequest.getListInt("idsAssociados").ToArray();

            var ViewModel = new PainelCobrancaVM();

            ViewModel.carregarDadosContribuicao(idContribuicao, idsAssociados);

            if (!ViewModel.listaNaoCobrados.Any()) {

                return Json(new { error = true, message = "Todos os associados informados já foram cobrados." }, JsonRequestBehavior.AllowGet);

            }
            if (ViewModel.listaNaoCobrados.Any(x => x.AssociadoContribuicao.dtVencimentoOriginal == DateTime.MinValue)){

                return Json(new { error = true, message = "Existem associados sem data de vencimento configurada para a geração da cobrança" }, JsonRequestBehavior.AllowGet);
                
            }


            return Json(new { error = false, message = $"O sistema irá realizar a cobrança de 0 associados. Isso poderá durar alguns minutos. Ao término da execução, você será notificado." }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Gerar as cobranças
        /// </summary>
        [ActionName("modal-enviar-cobrancas")]
        public ActionResult modalEnviarCobrancas() {

            var ViewModel = new ContribuicaoEnvioCobrancaForm();

            return View(ViewModel);
        }


    }
}