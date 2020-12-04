using System.Web.Mvc;
using WEB.Areas.Pedidos.ViewModels;

namespace WEB.Areas.Pedidos.Controllers {

    public class PedidoPorPeriodoWidgetController : Controller {

        [ActionName("widget-pedidos-por-periodo")]
        public PartialViewResult listarPedidosPorPeriodo() {

            var ViewModel = new WidgetPedidosPorPeriodoVM();
            
            ViewModel.capturarDatas();
            ViewModel.carregarDias();
            ViewModel.carregarDados();
            
            return PartialView("widget-pedidos-por-periodo-conteudo", ViewModel);

        }

    }
}