using System;
using System.Collections.Generic;
using System.Linq;
using BLL.Pedidos;
using BLL.Services;
using DAL.Pedidos;

namespace WEB.Areas.Pedidos.ViewModels {

    public class PedidoProducaoConsultaVM {

        //Atributos
        private IPedidoBL _IPedidoBL;
        private IPedidoEntregaBL _PedidoEntregaBL;

        //Propriedades
        private IPedidoBL OPedidoBL => _IPedidoBL = _IPedidoBL ?? new PedidoBL();
        private IPedidoEntregaBL OPedidoEntregaBL => _PedidoEntregaBL = _PedidoEntregaBL ?? new PedidoEntregaBL();
        
        //
        public PedidoProducaoConsultaVM() {
            
        }
        
        //
        public IQueryable<Pedido> montarQuery() {

            var valorBusca = UtilRequest.getString("valorBusca");

            var dtPrazoInicio = UtilRequest.getDateTime("dtPrazoInicio");
            var dtPrazoFim = UtilRequest.getDateTime("dtPrazoFim");

            var idsStatusPedido = UtilRequest.getListInt("idsStatusPedido");

            var query = this.OPedidoBL.listar(valorBusca, "S", 0);

            query = query.Where(x => x.idStatusPedido == StatusPedidoConst.PAGO ||
                                (x.idStatusPedido == StatusPedidoConst.EM_ABERTO && x.flagPagamentoNaEntrega == true) ||
                                (x.idStatusPedido == StatusPedidoConst.AGUARDANDO_PAGAMENTO && x.flagPagamentoNaEntrega == true));

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

        //
        public List<Pedido> filtrarCampos(IQueryable<Pedido> query) {

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
            
            var listaPedidoEntrega = this.OPedidoEntregaBL.listar().Where(x => idsPedidos.Contains(x.idPedido)).Select(e => new {
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
        
    }
    
}