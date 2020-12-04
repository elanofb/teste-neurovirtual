using System;
using System.Linq;
using System.Web.Mvc;
using BLL.Pedidos;
using PagedList;
using WEB.Areas.Pedidos.ViewModels;
using BLL.Organizacoes;
using DAL.Permissao.Security.Extensions;

namespace WEB.Areas.Pedidos.Controllers {

    [OrganizacaoFilter]
    public class PedidoProducaoController : Controller {

        //Atributos
        private IOrganizacaoBL _IOrganizacaoBL;
        private IPedidoProdutoBL _PedidoProdutoBL;

        //Propriedades
        private IOrganizacaoBL OOrganizacaoBL => _IOrganizacaoBL = _IOrganizacaoBL ?? new OrganizacaoBL();
        private IPedidoProdutoBL OPedidoProdutoBL => _PedidoProdutoBL = _PedidoProdutoBL ?? new PedidoProdutoBL();

        //
        public ActionResult index() {

            var OPedidoProducaoConsultaVM = new PedidoProducaoConsultaVM();

            var query = OPedidoProducaoConsultaVM.montarQuery();
            
            var queryFiltrada = OPedidoProducaoConsultaVM.filtrarCampos(query);
            
            var ViewModel = new PedidoProducaoVM();
            
            ViewModel.listaPedidos = queryFiltrada.OrderByDescending(x => x.id).ToPagedList(UtilRequest.getNroPagina(), UtilRequest.getNroRegistros());
            
            ViewModel.listaTodosPedidos = queryFiltrada;
            
            ViewModel.idsPedidos = ViewModel.listaTodosPedidos.Select(x => x.id).ToList();
            
            ViewModel.listaProdutos = this.OPedidoProdutoBL.listar(0).Where(x => ViewModel.idsPedidos.Contains(x.idPedido)).ToList();

            ViewModel.idBoxLista = "boxListaPedidosTodos";

            ViewModel.carregarTotais();

            return View(ViewModel);
        }

        //
        public ActionResult imprimir() {
            
            var OPedidoProducaoConsultaVM = new PedidoProducaoConsultaVM();

            var query = OPedidoProducaoConsultaVM.montarQuery();
            
            var queryFiltrada = OPedidoProducaoConsultaVM.filtrarCampos(query);

            var ViewModel = new PedidoProducaoImpressaoVM();
            
            ViewModel.DadosAssociacao = this.OOrganizacaoBL.carregar(User.idOrganizacao());
            
            ViewModel.listaTodosPedidos = queryFiltrada;   
            
            ViewModel.listaTodosPedidos.ForEach(OPedido => {
                OPedido.listaProdutos = this.OPedidoProdutoBL.listar(OPedido.id).ToList();
            });            

            return View(ViewModel);
        }

        [ActionName("imprimir-produtos")]
        public ActionResult imprimirProdutos() {
            
            var OPedidoProducaoConsultaVM = new PedidoProducaoConsultaVM();

            var query = OPedidoProducaoConsultaVM.montarQuery();
            
            var queryFiltrada = OPedidoProducaoConsultaVM.filtrarCampos(query);

            var ViewModel = new PedidoProducaoImpressaoVM();
            
            ViewModel.DadosAssociacao = this.OOrganizacaoBL.carregar(User.idOrganizacao());
            
            ViewModel.listaTodosPedidos = queryFiltrada;
            
            ViewModel.idsPedidos = ViewModel.listaTodosPedidos.Select(x => x.id).ToList();
            
            ViewModel.listaProdutos = this.OPedidoProdutoBL.listar(0).Where(x => ViewModel.idsPedidos.Contains(x.idPedido)).ToList();

            ViewModel.carregarResumo();

            return View(ViewModel);
        }

    }
    
}