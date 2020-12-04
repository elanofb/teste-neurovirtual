using System.Collections.Generic;
using BLL.Associados;
using DAL.Associados;
using DAL.Pedidos;

namespace WEB.Areas.Pedidos.Extensions {

    public static class PedidoExtensions {
        
        private static IAssociadoBL _AssociadoBL;
        
        private static IAssociadoBL OAssociadoBL => _AssociadoBL = _AssociadoBL ?? new AssociadoBL();
        
        //
        public static string exibirCorStatus(this Pedido OPedido) {

            string corAtivo = "bg-green";

            var listaStatusBgYellow = new List<int>() {
                StatusPedidoConst.EM_ABERTO, StatusPedidoConst.AGUARDANDO_PAGAMENTO, StatusPedidoConst.EM_PREPARACAO,
                StatusPedidoConst.PENDENTE, StatusPedidoConst.EM_ANDAMENTO, StatusPedidoConst.EM_MONTAGEM
            };

            if (listaStatusBgYellow.Contains(OPedido.idStatusPedido)) {
                corAtivo = "bg-yellow";
            }

            if (OPedido.idStatusPedido == StatusPedidoConst.CANCELADO) {
                corAtivo = "bg-red";
            }
            
            return corAtivo;

        }

        //
        public static string exibirCorTextoStatus(this Pedido OPedido) {

            string corAtivo = "text-green";

            var listaStatusBgYellow = new List<int>() {
                StatusPedidoConst.EM_ABERTO, StatusPedidoConst.AGUARDANDO_PAGAMENTO, StatusPedidoConst.EM_PREPARACAO,
                StatusPedidoConst.PENDENTE, StatusPedidoConst.EM_ANDAMENTO, StatusPedidoConst.EM_MONTAGEM
            };

            if (listaStatusBgYellow.Contains(OPedido.idStatusPedido)) {
                corAtivo = "text-yellow";
            }

            if (OPedido.idStatusPedido == StatusPedidoConst.CANCELADO) {
                corAtivo = "text-red";
            }
            
            return corAtivo;

        }

        //
        public static string classeBordaPedido(this Pedido OPedido) {

            string corAtivo = "border-green";

            var listaStatusBgYellow = new List<int>() {
                StatusPedidoConst.EM_ABERTO, StatusPedidoConst.AGUARDANDO_PAGAMENTO, StatusPedidoConst.EM_PREPARACAO,
                StatusPedidoConst.PENDENTE, StatusPedidoConst.EM_ANDAMENTO, StatusPedidoConst.EM_MONTAGEM
            };

            if (listaStatusBgYellow.Contains(OPedido.idStatusPedido)) {
                corAtivo = "border-yellow";
            }

            if (OPedido.idStatusPedido == StatusPedidoConst.CANCELADO) {
                corAtivo = "border-red";
            }
            
            return corAtivo;

        }
        
        //
        public static string exibirFlagAssociado(this Pedido OPedido) {

            var idPessoa = OPedido.idPessoa ?? 0;
            
            var Associado = OAssociadoBL.carregarAssociadoPessoa(idPessoa) ?? new Associado();

            if (Associado.id > 0) {
                return "Associado";
            }

            return "Não Associado";

        }
        
    }

}