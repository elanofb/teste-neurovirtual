using System;
using System.Linq;
using System.Web.Mvc;
using BLL.Pedidos;
using BLL.Services;
using DAL.Pedidos;
using PagedList;
using WEB.Areas.Pedidos.ViewModels;
using System.Collections.Generic;
using BLL.Organizacoes;
using DAL.Permissao.Security.Extensions;

namespace WEB.Areas.Pedidos.Controllers {

    [OrganizacaoFilter]
    public class PedidoMontagemController : Controller {

        //Atributos
        private IPedidoBL _IPedidoBL;
        private IOrganizacaoBL _IOrganizacaoBL;
        private IPedidoProdutoBL _PedidoProdutoBL;
        private IPedidoEntregaBL _PedidoEntregaBL;

        //Propriedades
        private IPedidoBL OPedidoBL => _IPedidoBL = _IPedidoBL ?? new PedidoBL();
        private IOrganizacaoBL OOrganizacaoBL => _IOrganizacaoBL = _IOrganizacaoBL ?? new OrganizacaoBL();
        private IPedidoProdutoBL OPedidoProdutoBL => _PedidoProdutoBL = _PedidoProdutoBL ?? new PedidoProdutoBL();
        private IPedidoEntregaBL OPedidoEntregaBL => _PedidoEntregaBL = _PedidoEntregaBL ?? new PedidoEntregaBL();

        //
        public ActionResult index() {
            var ViewModel = new PedidoMontagemVM();

            var query = this.montarQuery();
            
            var queryFiltrada = this.filtrarCampos(query);

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
            var ViewModel = new PedidoMontagemImpressaoVM();

            var query = this.montarQuery();
            var queryFiltrada = this.filtrarCampos(query);

            ViewModel.DadosAssociacao = this.OOrganizacaoBL.carregar(User.idOrganizacao());
            ViewModel.listaTodosPedidos = queryFiltrada;
            ViewModel.listaTodosPedidos.ForEach(OPedido => {
                OPedido.listaProdutos = this.OPedidoProdutoBL.listar(OPedido.id).ToList();
            });

            return View(ViewModel);
        }

        [ActionName("imprimir-produtos")]
        public ActionResult imprimirProdutos() {
            var ViewModel = new PedidoMontagemImpressaoVM();

            var query = this.montarQuery();
            var queryFiltrada = this.filtrarCampos(query);

            ViewModel.DadosAssociacao = this.OOrganizacaoBL.carregar(User.idOrganizacao());
            ViewModel.listaTodosPedidos = queryFiltrada;
            ViewModel.idsPedidos = ViewModel.listaTodosPedidos.Select(x => x.id).ToList();
            ViewModel.listaProdutos = this.OPedidoProdutoBL.listar(0).Where(x => ViewModel.idsPedidos.Contains(x.idPedido)).ToList();

            ViewModel.carregarResumo();

            return View(ViewModel);
        }

        #region NONACTIONS

        [NonAction]
        private IQueryable<Pedido> montarQuery() {

            var valorBusca = UtilRequest.getString("valorBusca");

            var dtPrazoInicio = UtilRequest.getDateTime("dtPrazoInicio");
            var dtPrazoFim = UtilRequest.getDateTime("dtPrazoFim");

            var query = this.OPedidoBL.listar(valorBusca, "S", 0);

            query = query.Where(x => x.idStatusPedido == StatusPedidoConst.EM_MONTAGEM);

            if (dtPrazoInicio.HasValue) {
                query = query.Where(x => x.listaPedidoEntrega.Any(c => c.dtAgendamentoEntrega >= dtPrazoInicio));
            }

            if (dtPrazoFim.HasValue) {
                var dtFiltro = dtPrazoFim.Value.AddDays(1);
                query = query.Where(x => x.listaPedidoEntrega.Any(c => c.dtAgendamentoEntrega < dtFiltro));
            }


            return query;
        }

        [NonAction]
        private List<Pedido> filtrarCampos(IQueryable<Pedido> query) {

            var queryFiltrada = query.Select(x => new {
                x.id,
                x.nomePessoa,
                x.cpf,
                x.dtCadastro,
                x.dtFinalizado,
                x.valorProdutos,
                x.valorFrete,
                x.StatusPedido,
                x.idStatusPedido,
                x.flagPagamentoNaEntrega
            }).ToListJsonObject<Pedido>();

            var idsPedidos = queryFiltrada.Select(x => x.id).ToList();
            var listaPedidoEntrega = OPedidoEntregaBL.listar().Where(x => idsPedidos.Contains(x.idPedido)).Select(e => new {
                e.id,
                e.idPedido,
                e.dtAgendamentoEntrega,
                e.flagPeriodo,
                e.cep,
                e.logradouro,
                e.numero,
                e.complemento,
                e.bairro,
                Cidade = new { e.Cidade.nome, Estado = new { e.Cidade.Estado.sigla } },
            }).ToListJsonObject<PedidoEntrega>();
            
            queryFiltrada.ForEach(Item => Item.listaPedidoEntrega = listaPedidoEntrega.Where(x => x.idPedido == Item.id).OrderByDescending(x => x.id).ToList());
            
            return queryFiltrada;
        }

        #endregion
    }
}