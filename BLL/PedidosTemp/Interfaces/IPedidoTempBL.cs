using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL.Services;
using DAL.PedidosTemp;
using EntityFramework.Extensions;

namespace BLL.PedidosTemp {

    public interface IPedidoTempBL {
        
        IQueryable<PedidoTemp> query();
            
        PedidoTemp carregar(string idSessao);
        
        bool salvar(PedidoTemp OPedidoTemp);
        
    }

}
