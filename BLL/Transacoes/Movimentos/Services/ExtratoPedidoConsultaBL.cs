using System;
using System.Linq;
using BLL.Services;
using DAL.Transacoes;

namespace BLL.Transacoes.Movimentos {

    public class ExtratoPedidoConsultaBL : DefaultBL, IExtratoPedidoConsultaBL {


        /// <summary>
        /// 
        /// </summary>
        public IQueryable<MovimentoResumoPedido> query(int idPedido, int idPedidoProduto, DateTime? dtInicio, DateTime? dtFim) {

            var query = from Mov in db.Movimento
                        join Pp in db.PedidoProduto on Mov.idPedidoProduto equals Pp.id
                        join Ped in db.Pedido on Pp.idPedido equals Ped.id
                        join Prod in db.Produto on Pp.idProduto equals Prod.id
                        select new MovimentoResumoPedido {
                                                             idMovimento = Mov.id, 
                                                             dtCadastro = Mov.dtCadastro, 
                                                             observacao = Mov.observacao, 
                                                             valorOperacao = Mov.valor,
                                                             dtIntegracaoSaldo = Mov.dtIntegracaoSaldo,
                                                             idTipoTransacao = Mov.idTipoTransacao,
                                                             idMembroDestino = Mov.idMembroDestino,
                                                             dtPedidoQuitacao = Ped.dtQuitacao,
                                                             idPedido = Pp.idPedido,
                                                             idPedidoProduto = Pp.id,
                                                             nomeProduto = Prod.nome
                                                         };

            if (idPedido > 0) {
                query = query.Where(x => x.idPedido == idPedido);
            }

            if (idPedidoProduto > 0) {
                query = query.Where(x => x.idPedidoProduto == idPedidoProduto);
            }

            if (dtInicio.HasValue) {
                
                var dtFiltro = dtInicio.Value;
                
                query = query.Where(x => x.dtCadastro >= dtFiltro);
            }

            if (dtFim.HasValue) {
                
                var dtFiltro = dtFim.Value;
                
                query = query.Where(x => x.dtCadastro < dtFiltro);
            }
            
                                                 
            return query;
        }
    }

}
