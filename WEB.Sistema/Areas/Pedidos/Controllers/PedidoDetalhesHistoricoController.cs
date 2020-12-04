using System.Linq;
using System.Web.Mvc;
using BLL.Pedidos;
using WEB.Areas.Pedidos.ViewModels;

namespace WEB.Areas.Pedidos.Controllers {

    public class PedidoDetalhesHistoricoController : Controller {

        //Constantes

        //Atributos
        private IPedidoHistoricoBL _IPedidoHistoricoBL;

        //Propriedades
        private IPedidoHistoricoBL OPedidoHistoricoBL => _IPedidoHistoricoBL = _IPedidoHistoricoBL ?? new PedidoHistoricoBL();

        // 
        [ActionName("partial-lista-ocorrencias")]
        public PartialViewResult partialListaOcorrencias(int idPedido) {

            var ViewModel = new PedidoDetalhesHistoricoVM();

            ViewModel.idPedido = idPedido;

            ViewModel.listaOcorrencias = OPedidoHistoricoBL.listar(idPedido).OrderByDescending(x => x.id).ToList();

            return PartialView(ViewModel);

        }

    }

}