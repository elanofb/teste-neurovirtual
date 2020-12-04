using System;
using System.Collections.Generic;
using System.Linq;
using BLL.Pedidos;
using BLL.Services;
using DAL.Pedidos;
using PagedList;

namespace WEB.Areas.Pedidos.ViewModels{
    
    public class PedidoConsultaVM {
        
        //Atributos Serviços
        private IPedidoBL _IPedidoBL;

        //Propriedades  Serviços
        private IPedidoBL OPedidoBL => _IPedidoBL = _IPedidoBL ?? new PedidoBL();
        
        // Propriedades
        public IPagedList<Pedido> listaPedidos { get; set; }

        public string idBoxLista { get; set; }
        
        public bool flagPodeCancelar { get; set; }

        //
        public PedidoConsultaVM() {
            this.listaPedidos = new List<Pedido>().ToPagedList(1, 20);
        }
        
        //
        public IQueryable<Pedido> montarQuery() {

            var valorBusca = UtilRequest.getString("valorBusca");

            var dtCadastroInicio = UtilRequest.getDateTime("dtCadastroInicio");
            var dtCadastroFim = UtilRequest.getDateTime("dtCadastroFim");

            var dtPrazoInicio = UtilRequest.getDateTime("dtPrazoInicio");
            var dtPrazoFim = UtilRequest.getDateTime("dtPrazoFim");

            var idsStatusPedido = UtilRequest.getListInt("idsStatusPedido");

            var query = this.OPedidoBL.listar(valorBusca, "S", 0);

            if (dtCadastroInicio.HasValue) {
                query = query.Where(x => x.dtCadastro >= dtCadastroInicio);
            }

            if (dtCadastroFim.HasValue) {
                var dtFiltro = dtCadastroFim.Value.AddDays(1);
                query = query.Where(x => x.dtCadastro < dtFiltro);
            }

            if (dtPrazoInicio.HasValue) {
                query = query.Where(x => x.listaPedidoEntrega.Any(c => c.dtAgendamentoEntrega >= dtPrazoInicio));
            }

            if (dtPrazoFim.HasValue) {
                var dtFiltro = dtPrazoFim.Value.AddDays(1);
                query = query.Where(x => x.listaPedidoEntrega.Any(c => c.dtAgendamentoEntrega < dtFiltro));
            }

            if (idsStatusPedido?.Any() == true) {
                query = query.Where(x => idsStatusPedido.Contains(x.idStatusPedido));
            }            

            return query;

        }
        
        public List<Pedido> filtrarCampos(IQueryable<Pedido> query) {

            var queryFiltrada = query.Select(x => new {
                x.id,
                x.idAssociado,
                x.idNaoAssociado,
                x.nomePessoa,
                x.cpf,
                x.dtCadastro,
                x.dtFinalizado,
                x.valorProdutos,
                x.valorFrete,
                x.StatusPedido,
                x.idStatusPedido,
                x.flagPagamentoNaEntrega,
                Associado = new{
                    x.Associado.nroAssociado
                },
                NaoAssociado = new{
                    x.NaoAssociado.nroAssociado
                },
                listaPedidoEntrega = x.listaPedidoEntrega.OrderByDescending(e => e.id).Select(c => new { c.dtAgendamentoEntrega, c.flagPeriodo }).Take(1)
            }).ToListJsonObject<Pedido>();
            
            return queryFiltrada;

        }
        
	}

}