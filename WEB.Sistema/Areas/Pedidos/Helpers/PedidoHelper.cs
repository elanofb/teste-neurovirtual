using System;
using System.Collections.Generic;
using System.Web.Mvc;
using UTIL.UtilClasses;

namespace WEB.Areas.Pedidos.Helpers {

    public class PedidoHelper {

        //Constanctes
	    private static PedidoHelper _instance;

        //Atributos

        //Propriedades
	    public static PedidoHelper getInstance => _instance = _instance ?? new PedidoHelper();

		//
		public SelectList selectListParcelamento(int? selected) {


		    int limite = /*Config.limiteParcelamentoPedido ??*/ 1;

		    var listaOpcoes = new List<OptionSelect>();

           listaOpcoes.Add(new OptionSelect { value = "1", text = "Á vista" });

		    for (int i = 2; i <= limite; i++) {
		        listaOpcoes.Add(new OptionSelect { value = i.ToString(), text = String.Concat(i, " parcelas") });
		    }


            return new SelectList(listaOpcoes, "value", "text", selected);
		}
    }
}