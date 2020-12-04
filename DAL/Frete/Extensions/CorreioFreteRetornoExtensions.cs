using System;
using System.Linq;
using System.Collections.Generic;

namespace DAL.Frete {

    public static class CorreioFreteRetornoExtensions{
		
		//
        public static string descricaoTipoFrete(this CorreiosFreteRetorno OFrete){

			if (OFrete.codigoServico == CorreiosTipoFreteConst.PAC) {
				return "Entrega Padrão (PAC)";
			}
			
			if (OFrete.codigoServico == CorreiosTipoFreteConst.SEDEX) {
				return "Sedex";
			}

			return "-";
        }

		//Adicionar 2 dias ao prazo de entrega padrão para ter margem para atrasos
        public static int prazoEntregaTratado(this CorreiosFreteRetorno OFrete){

			int qtdeDias = UtilNumber.toInt32(OFrete.prazoEntrega);

			return (qtdeDias + 2);
        }
	}
}
