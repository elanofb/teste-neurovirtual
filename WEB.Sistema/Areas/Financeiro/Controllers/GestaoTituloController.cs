using System;
using System.Linq;
using System.Web.Mvc;
using BLL.Financeiro;
using DAL.Financeiro;
using WEB.Areas.Financeiro.ViewModels;

namespace WEB.Areas.Financeiro.Controllers {

    public class GestaoTituloController : Controller {
        
        //Atributos
        private IGestaoTituloVWBL _GestaoTituloVWBL;

        //Propriedades
        private IGestaoTituloVWBL OGestaoTituloVWBL { get { return (this._GestaoTituloVWBL = this._GestaoTituloVWBL ?? new GestaoTituloVWBL()); } }

        //
        public ActionResult listar() {

            var nf = UtilRequest.getString("nf");
            var destinatario = UtilRequest.getString("destinatario");
            var valorBusca = UtilRequest.getString("valorBusca");
            var flagPago = UtilRequest.getString("flagPago");
            var pesquisarPor = UtilRequest.getString("pesquisarPor");
            DateTime? dtInicio = UtilRequest.getDateTime("dtInicio") ?? DateTime.Today;
            DateTime? dtFim = UtilRequest.getDateTime("dtFim") ?? DateTime.Today;

            var listaExtrato = this.OGestaoTituloVWBL.listar(valorBusca, destinatario, nf, flagPago, pesquisarPor, dtInicio, dtFim).OrderBy(x => x.dtVencimento).ToList();

            var VM = new GestaoTituloVM();
            VM.listaExtrato = listaExtrato;
            VM.valorTotalReceitas = listaExtrato.Where(x => x.tipo == TipoTituloConst.RECEITA).Sum(x => (decimal?) x.valorPago) ?? 0M;
            VM.valorTotalDespesas = listaExtrato.Where(x => x.tipo == TipoTituloConst.DESPESA).Sum(x => (decimal?) x.valorPago) ?? 0M;
            VM.valorTotal = (VM.valorTotalReceitas - VM.valorTotalDespesas);
            VM.validarArquivo();

            ViewBag.dtInicio = dtInicio.Value.ToShortDateString();
            ViewBag.dtFim = dtFim.Value.ToShortDateString();

            return View(VM);
        }
    }
}
