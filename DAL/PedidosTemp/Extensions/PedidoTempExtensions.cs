using System;
using System.Collections.Generic;
using System.Linq;

namespace DAL.PedidosTemp.Extensions {

    public static class PedidoTempExtensions{

		/**
		 * Extension method para capturar valor total dos items de uma lista
		 */
		public static decimal getValorTotal(this List<PedidoProdutoTemp> lista){

            if(lista == null) {
                return new decimal(0);
            }

			return lista.Select(x => (x.valorItem * x.qtde) ).Sum();
		}

		/**
		 * Extension method para capturar valor total dos items de uma lista
		 */
		public static decimal getPesoTotal(this List<PedidoProdutoTemp> lista){

            if(lista?.Any() == false) {
                return new decimal(0) ;
            }

			return lista.Select(x => x.qtde * x.peso.toDecimal()).Sum();

		}


		/**
		 * Extension method para capturar valor total dos items de uma lista
		 */
		public static int getQtdeItens(this List<PedidoProdutoTemp> lista){

            if(lista == null) {
                return 0;
            }

            return lista.Select(x => x.qtde).Sum();

		}
        
    }

}