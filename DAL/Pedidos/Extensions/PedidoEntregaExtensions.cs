using System;
using System.Collections.Generic;
using System.Linq;
using DAL.Financeiro;
using DAL.Pedidos;
using DAL.Pessoas;

namespace DAL.Pedidos.Extensions {

    public static class PedidoEntregaExtensions {
       
        /// <summary>
        /// Preencher os dados da pessoa no pedido
        /// </summary>
        public static string exibirPeriodoEntrega(this PedidoEntrega OPedidoEntrega){

            if (OPedidoEntrega.flagPeriodo == PeriodoEntregaConst.MANHA){
                return "Manhã";
            }

            if (OPedidoEntrega.flagPeriodo == PeriodoEntregaConst.TARDE) {
                return "Tarde";
            }

            if (OPedidoEntrega.flagPeriodo == PeriodoEntregaConst.INTEGRAL) {
                return "Integral";
            }

            return "Não informado";
        }
    }
}