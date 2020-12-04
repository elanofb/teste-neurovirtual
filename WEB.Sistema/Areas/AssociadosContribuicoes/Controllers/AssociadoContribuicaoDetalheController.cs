using System.Linq;
using System.Web.Mvc;
using BLL.AssociadosContribuicoes;
using BLL.Financeiro;
using BLL.Notificacoes;
using DAL.Financeiro;
using DAL.Notificacoes;
using WEB.App_Infrastructure;

namespace WEB.Areas.AssociadosContribuicoes.Controllers {

    public class AssociadoContribuicaoDetalheController : BaseSistemaController {

        //Atributos
        private IReceitaConsultaBL _ReceitaConsultaBL;
        private IAssociadoContribuicaoBL _AssociadoContribuicaoBL;
        private INotificacaoSistemaEnvioBL _NotificacaoSistemaEnvioBL;

        //Propriedades
        private IReceitaConsultaBL OReceitaConsultaBL => this._ReceitaConsultaBL = this._ReceitaConsultaBL ?? new ReceitaConsultaBL();
        private IAssociadoContribuicaoBL OAssociadoContribuicaoBL => this._AssociadoContribuicaoBL = this._AssociadoContribuicaoBL ?? new AssociadoContribuicaoBL();
        private INotificacaoSistemaEnvioBL ONotificacaoSistemaEnvioBL => this._NotificacaoSistemaEnvioBL = this._NotificacaoSistemaEnvioBL ?? new NotificacaoSistemaEnvioBL();


        //Formulario submetido para novo cargo para o associado
        public ActionResult index(int id) {

            var OAssociadoContribuicao = OAssociadoContribuicaoBL.carregar(id);

            if (OAssociadoContribuicao == null) {
                return RedirectToAction("index", "PainelCobranca", new {area = "ContribuicoesPainel"});
            }

            return View(OAssociadoContribuicao);
        }

        [ActionName("partial-lista-email-associado-cobranca")]
        public ActionResult partialListaEmailAssociadoCobranca(int id) {
            
            var listaEmails = ONotificacaoSistemaEnvioBL.listar(id, 0)
                                .Where(x => x.NotificacaoSistema.idTipoNotificacao == TipoNotificacaoConst.COBRANCA_PADRAO)
                                .OrderByDescending(x => x.id).ToList();

            return View(listaEmails);
        }

        [ActionName("partial-lista-titulo-receita-pagamento")]
        public ActionResult partialListaTituloReceitaPagamento(int id) {

            var listaTituloReceitaPagamento = OReceitaConsultaBL.listarPagamentos(TipoReceitaConst.CONTRIBUICAO, true).Where(x => x.idReceita == id).ToList();

            return View(listaTituloReceitaPagamento);
        }
    }
}