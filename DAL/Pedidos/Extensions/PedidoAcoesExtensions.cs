namespace DAL.Pedidos.Extensions {
    
    public static class PedidoAcoesExtensions {
        
        /// <summary>
        /// Verifica se é possivel faturar o pedido
        /// </summary>
	    public static bool flagTemParcelamento(this Pedido OPedido) {

            if (OPedido.TituloReceita.listaTituloReceitaPagamento.Count > 1) {
                return true;
            }

            return false;
        }
        
        /// <summary>
        /// Verifica se é possivel faturar o pedido
        /// </summary>
	    public static bool flagFaturavel(this Pedido OPedido) {

            if (OPedido.dtFaturamento.HasValue) {
                return false;
            }

            if (OPedido.dtCancelamento.HasValue) {
                return false;
            }


            return true;
        }

        /// <summary>
        /// Verifica se é possivel dar baixa ou gerar link de pagamento para o pedido
        /// </summary>
	    public static bool flagPagavel(this Pedido OPedido) {

            if (OPedido.TituloReceita == null || OPedido.TituloReceita.id == 0) {
                return false;
            }

            if (!OPedido.dtFaturamento.HasValue) {
                return false;
            }

            if (OPedido.dtCancelamento.HasValue) {
                return false;
            }

            if (OPedido.getValorTotal() == 0) {
                return false;
            }

            return true;
        }

        //
        public static bool flagPago(this Pedido OPedido) {

            if (!OPedido.dtQuitacao.HasValue) {
                return false;
            }

            if (OPedido.dtCancelamento.HasValue) {
                return false;
            }

            return true;

        }

        // 
        public static bool flagProntoParaAtender(this Pedido OPedido) {

            if (!OPedido.flagPago()) {
                return false;
            }

            if (OPedido.dtAtendimento.HasValue) {
                return false;
            }

            if (OPedido.dtExpedicao.HasValue) {
                return false;
            }

            if (OPedido.dtFinalizado.HasValue) {
                return false;
            }

            return true;

        }

        // 
        public static bool flagProntoParaPreparar(this Pedido OPedido) {

            if (!OPedido.flagPago()) {
                return false;
            }

            if (OPedido.dtPreparacao.HasValue) {
                return false;
            }

            if (OPedido.dtExpedicao.HasValue) {
                return false;
            }

            if (OPedido.dtFinalizado.HasValue) {
                return false;
            }

            return true;

        }

        //
        public static bool flagProntoParaExpedir(this Pedido OPedido) {

            if (!OPedido.flagPago()) {
                return false;
            }

            if (OPedido.dtExpedicao.HasValue) {
                return false;
            }

            if (OPedido.dtFinalizado.HasValue) {
                return false;
            }
            
            return true;

        }

        //
        public static bool flagFinalizavel(this Pedido OPedido) {

            if (!OPedido.flagPago()) {
                return false;
            }

            if (OPedido.dtFinalizado.HasValue) {
                return false;
            }
            
            return true;

        }

    }

}