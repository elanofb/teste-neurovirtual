using System;

namespace DAL.Pedidos {

    public static class StatusPedidoConst {
        public static readonly int EM_ABERTO = 1;

        public static readonly int AGUARDANDO_PAGAMENTO = 2;

        public static readonly int PAGO = 3;

        public static readonly int EM_PREPARACAO = 4;

        public static readonly int ATENDIDO = 5;

        public static readonly int EXPEDIDO = 6;

        public static readonly int FINALIZADO = 7;

        public static readonly int CANCELADO = 8;

        public static readonly int PENDENTE = 9;

        public static readonly int EM_ANDAMENTO = 10;

        public static readonly int EM_MONTAGEM = 11;

        public static readonly int AGUARDANDO_EXPEDICAO = 12;
    }
}