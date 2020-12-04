using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BLL.Frete;
using BLL.PedidosTemp;
using BLL.Pessoas;
using BLL.Services;
using DAL.Pedidos;
using DAL.PedidosTemp;
using DAL.Pessoas;

namespace BLL.Pedidos {

    public interface IPedidoEntregaOperacaoBL {
        
		bool salvar(PedidoEntrega OPedidoEntrega);

	}

}