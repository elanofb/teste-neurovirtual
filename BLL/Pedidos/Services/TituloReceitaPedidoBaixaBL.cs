using System;
using System.Linq;
using BLL.Core.Events;
using BLL.Financeiro;
using BLL.Services;
using BLL.Transacoes.Compras;
using DAL.Financeiro;
using DAL.Pedidos;
using EntityFramework.Extensions;

namespace BLL.Pedidos {

    public class TituloReceitaPedidoBaixaBL : TituloReceitaBaixaBL {

        //Atributos
        private ITituloReceitaPagamentoConsultaBL _Pagamento;
        private ICompraFacade _CompraFacade;
        private IMediadorBase _MediadorCompra;
        
        //Propriedades
        private ITituloReceitaPagamentoConsultaBL PagamentoConsultaBL => _Pagamento = _Pagamento ?? new TituloReceitaPagamentoConsultaBL();
        private ICompraFacade CompraFacade => _CompraFacade = _CompraFacade ?? new CompraFacade();
        private IMediadorBase MediadorCompra => _MediadorCompra = _MediadorCompra ?? new MediadorCompra();

        //Eventos


        //Construtor
        public TituloReceitaPedidoBaixaBL() {
        }


        /// <summary>
        /// Registrar a liquidacao do titulo (pagamento total)
        /// Registrar a quitacao no pedido também alteração do status
        /// </summary>
        public override TituloReceita liquidar(TituloReceita OTituloReceita) {

            var OTituloLiquidado = base.liquidar(OTituloReceita);

            if (!OTituloLiquidado.dtQuitacao.HasValue) {

                return OTituloLiquidado;
            }

            var OPagamento = PagamentoConsultaBL.query(1)
                                                .Where(x => x.idTituloReceita == OTituloLiquidado.id)
                                                .Select(x => new {x.id, x.idTituloReceita, x.idMeioPagamento})
                                                .FirstOrDefault();

            byte? idMeioPagamentoParam = null;

            if (OPagamento != null) {

                idMeioPagamentoParam = OPagamento.idMeioPagamento;
            }
                                    
            var Movimento = this.MediadorCompra.carregarDados(OTituloReceita.idPessoa.toInt(), 1, OTituloReceita.valorTotalComDesconto(), OTituloReceita.idReceita.toInt());
            
            Movimento.flagPagamentoComBitkink = idMeioPagamentoParam == MeioPagamentoConst.BITLINK;
            
            Movimento.flagIgnorarSaldo = Movimento.flagPagamentoComBitkink == false;

            UtilRetorno ORetorno = this.CompraFacade.pagar(Movimento);

            if (ORetorno.flagError){
                return OTituloLiquidado;
            }
            
            this.atualizarPedido(OTituloLiquidado, idMeioPagamentoParam);

            return OTituloLiquidado;

        }
        
        /// <summary>
        /// 
        /// </summary>
        private void atualizarPedido(TituloReceita OTituloLiquidado, byte? idMeioPagamentoParam) {
            
            //Atualizar Pedido
            db.Pedido.Where(x => x.id == OTituloLiquidado.idReceita)
              .Update(x => new Pedido {
                                          dtQuitacao = OTituloLiquidado.dtQuitacao,
                                          idStatusPedido = StatusPedidoConst.PAGO,
                                          idMeioPagamento = idMeioPagamentoParam
                                      });

            //Atualizar os itens do pedido
            var listaItens = db.PedidoProduto.Where(x => x.idPedido == OTituloLiquidado.idReceita)
                               .Select(x => new {x.id, x.valorGanhoDiario, x.qtdeDiasDuracao})
                               .ToListJsonObject<PedidoProduto>();

            foreach (var Item in listaItens) {

                var dtFimGanho = DateTime.Today.AddDays(Item.qtdeDiasDuracao.toInt());
                
                db.PedidoProduto.Where(x => x.id == Item.id)
                  .Update(x => new PedidoProduto {
                                                     dtFimGanhoDiario = dtFimGanho
                                                 });
                
            }

        }

    }

}
