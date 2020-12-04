using System;
using System.Collections.Generic;
using System.Linq;
using BLL.Core.Events;
using BLL.Services;
using DAL.Pedidos;
using EntityFramework.Extensions;

namespace BLL.Pedidos {

    public interface IPedidoAcaoAtendimentoBL {

        void atender(int idPedido, string observacoes);

	}

}