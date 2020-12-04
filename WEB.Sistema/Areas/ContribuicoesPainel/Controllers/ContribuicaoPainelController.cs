using System;
using System.Linq;
using System.Web.Mvc;
using BLL.Contribuicoes;
using DAL.Contribuicoes;
using MvcFlashMessages;
using WEB.Areas.ContribuicoesPainel.ViewModels;

namespace WEB.Areas.ContribuicoesPainel.Controllers {

    [OrganizacaoFilter]
    public class ContribuicaoPainelController : Controller {

        //Atributos
        private IContribuicaoBL _ContribuicaoBL;

        //Servicos
        private IContribuicaoBL OContribuicaoBL => _ContribuicaoBL = _ContribuicaoBL ?? new ContribuicaoPadraoBL();

        // GET: Contribuicoes/ContribuicaoPainel
        public ActionResult Index(int id) {

            var OContribuicao = this.OContribuicaoBL.carregar(id);

            if (OContribuicao == null) {
                return RedirectToAction("listar", "Contribuicao");
            }

            if (OContribuicao.idPeriodoContribuicao == PeriodoContribuicaoConst.ANUAL) {

                return RedirectToAction("painel-anuidade", new {id});

            }

            return RedirectToAction("painel-contribuicao", new {id});
        }

        // GET: Contribuicoes/ContribuicaoPainel
        [ActionName("painel-contribuicao")]
        public ActionResult painelContribuicao(int id) {

            var listaDatas = UtilRequest.getListString("dtVencimento");

            if (listaDatas.Count > 12) {
                this.Flash(UtilMessage.TYPE_MESSAGE_INFO, "A quantidade de períodos visíveis no painel está limitada a 12.");
            }

            var OContribuicao = this.OContribuicaoBL.carregar(id);

            if (OContribuicao == null) {
                return RedirectToAction("listar", "Contribuicao");
            }

            var ViewModel = new PainelContribuicaoPadraoVM();

            ViewModel.listaDatasSelecionadas = listaDatas.Select(UtilDate.cast)
                                                        .Where(x => x > DateTime.MinValue)
                                                        .ToList();

            ViewModel.Contribuicao = OContribuicao;

            ViewModel.carregarDadosContribuicao();

            return View(ViewModel);
        }

        // GET: Contribuicoes/ContribuicaoPainel
        [ActionName("painel-anuidade")]
        public ActionResult painelAnuidade(int id) {

            var OContribuicao = this.OContribuicaoBL.carregar(id);



            return View();
        }
    }
}