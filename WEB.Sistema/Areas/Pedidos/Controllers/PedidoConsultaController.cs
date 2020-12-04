using System;
using System.Linq;
using System.Web.Mvc;
using DAL.Pedidos;
using PagedList;
using WEB.Areas.Pedidos.ViewModels;

namespace WEB.Areas.Pedidos.Controllers {

    [OrganizacaoFilter]
    public class PedidoConsultaController : Controller {

        //
        public ActionResult index() {
            return View();
        }
        
        //
        [ActionName("em-aberto")]
        public PartialViewResult emAberto() {

            var ViewModel = new PedidoConsultaVM();
            
            var query = ViewModel.montarQuery().Where(x => x.StatusPedido.flagFinalizador == "N" && x.dtQuitacao == null && x.flagPagamentoNaEntrega != true);
            
            var queryFiltrada = ViewModel.filtrarCampos(query);

            ViewModel.listaPedidos = queryFiltrada.OrderByDescending(x => x.id).ToPagedList(UtilRequest.getNroPagina(), UtilRequest.getNroRegistros());

            ViewModel.idBoxLista = "boxListaPedidosEmAberto";

            ViewModel.flagPodeCancelar = true;
            
            return PartialView("partial-lista", ViewModel);

        }

        //
        public PartialViewResult pagos() {

            var ViewModel = new PedidoConsultaVM();
            
            var query = ViewModel.montarQuery().Where(x => 
                                                           (x.dtQuitacao.HasValue || x.flagPagamentoNaEntrega == true) && 
                                                          !x.dtAtendimento.HasValue &&
                                                          !x.dtPreparacao.HasValue &&
                                                          !x.dtExpedicao.HasValue &&
                                                          !x.dtCancelamento.HasValue && 
                                                           x.StatusPedido.flagFinalizador == "N");
            
            var queryFiltrada = ViewModel.filtrarCampos(query);

            ViewModel.listaPedidos = queryFiltrada.OrderByDescending(x => x.id).ToPagedList(UtilRequest.getNroPagina(), UtilRequest.getNroRegistros());

            ViewModel.idBoxLista = "boxListaPedidosPagos";

            return PartialView("partial-lista", ViewModel);

        }
        
        //
        [ActionName("em-producao")]
        public PartialViewResult emProducao() {

            var ViewModel = new PedidoConsultaVM();
            
            var query = ViewModel.montarQuery().Where(x => (x.dtPreparacao.HasValue || x.dtAtendimento.HasValue) &&
                                                           !x.dtExpedicao.HasValue &&
                                                           !x.dtCancelamento.HasValue &&
                                                           x.StatusPedido.flagFinalizador == "N");
            
            var queryFiltrada = ViewModel.filtrarCampos(query);

            ViewModel.listaPedidos = queryFiltrada.OrderByDescending(x => x.id).ToPagedList(UtilRequest.getNroPagina(), UtilRequest.getNroRegistros());

            ViewModel.idBoxLista = "boxListaPedidosEmProducao";

            return PartialView("partial-lista", ViewModel);

        }
        
        //
        [ActionName("em-transporte")]
        public PartialViewResult emTransporte() {

            var ViewModel = new PedidoConsultaVM();
            
            var query = ViewModel.montarQuery().Where(x => x.dtExpedicao.HasValue &&
                                                           !x.dtCancelamento.HasValue &&
                                                           x.StatusPedido.flagFinalizador == "N");
            
            var queryFiltrada = ViewModel.filtrarCampos(query);

            ViewModel.listaPedidos = queryFiltrada.OrderByDescending(x => x.id).ToPagedList(UtilRequest.getNroPagina(), UtilRequest.getNroRegistros());

            ViewModel.idBoxLista = "boxListaPedidosEmTransporte";

            return PartialView("partial-lista", ViewModel);

        }

        //
        public PartialViewResult finalizados() {

            var ViewModel = new PedidoConsultaVM();
            
            int idStatusCancelado = StatusPedidoConst.CANCELADO;

            var query = ViewModel.montarQuery().Where(x => (x.StatusPedido.flagFinalizador == "S" || x.dtFinalizado.HasValue) && 
                                                           x.idStatusPedido != idStatusCancelado);
            
            var queryFiltrada = ViewModel.filtrarCampos(query);

            ViewModel.listaPedidos = queryFiltrada.OrderByDescending(x => x.id).ToPagedList(UtilRequest.getNroPagina(), UtilRequest.getNroRegistros());

            ViewModel.idBoxLista = "boxListaPedidosFinalizados";

            return PartialView("partial-lista", ViewModel);

        }
        
        //
        public PartialViewResult atrasados() {

            var ViewModel = new PedidoConsultaVM();
            
            var query = ViewModel.montarQuery().Where(x => x.StatusPedido.flagFinalizador == "N" &&
                                                           x.dtQuitacao.HasValue &&
                                                           x.listaPedidoEntrega.Where(c => c.flagExcluido == "N").Select(c => c.dtAgendamentoEntrega).Max() < DateTime.Today);
            
            var queryFiltrada = ViewModel.filtrarCampos(query);

            ViewModel.listaPedidos = queryFiltrada.OrderByDescending(x => x.id).ToPagedList(UtilRequest.getNroPagina(), UtilRequest.getNroRegistros());

            ViewModel.idBoxLista = "boxListaPedidosAtrasados";

            return PartialView("partial-lista", ViewModel);

        }

        //
        public PartialViewResult cancelados() {

            var ViewModel = new PedidoConsultaVM();
            
            int idStatusCancelado = StatusPedidoConst.CANCELADO;

            var query = ViewModel.montarQuery().Where(x => x.idStatusPedido == idStatusCancelado);
            
            var queryFiltrada = ViewModel.filtrarCampos(query);
            
            ViewModel.listaPedidos = queryFiltrada.OrderByDescending(x => x.id).ToPagedList(UtilRequest.getNroPagina(), UtilRequest.getNroRegistros());

            ViewModel.idBoxLista = "boxListaPedidosCancelados";

            return PartialView("partial-lista", ViewModel);

        }

    }

}