using System;
using System.Data.Entity;
using System.Linq;
using DAL.Pedidos;
using DAL.Repository.Base;
using EntityFramework.Extensions;
using BLL.Services;

namespace BLL.Pedidos {

	public class PedidoEntregaBL : DefaultBL, IPedidoEntregaBL {
		
		//
		public IQueryable<PedidoEntrega> listar() {

			var query = from Obj in this.db.PedidoEntrega
				where Obj.flagExcluido == "N"
				select Obj;

			query = query.condicoesSeguranca();
            
			return query;
		}
		
        //
		public bool salvar(PedidoEntrega OPedidoEntrega) {
            
            OPedidoEntrega.Cidade = null;
            OPedidoEntrega.Estado = null;
            OPedidoEntrega.Pais = null;
            OPedidoEntrega.Pedido = null;
            OPedidoEntrega.TipoEndereco = null;
            OPedidoEntrega.Transportador = null;
			
		    OPedidoEntrega.setDefaultInsertValues();

		    db.PedidoEntrega.Add(OPedidoEntrega);

		    db.SaveChanges();

		    bool flagSucesso = OPedidoEntrega.id > 0;
            
		    if (flagSucesso) {
		        db.PedidoEntrega
		            .Where(x => x.flagExcluido == "N" && x.idPedido == OPedidoEntrega.idPedido && x.id != OPedidoEntrega.id)
		            .Update(x => new PedidoEntrega { flagExcluido = "S" });
		    }

		    return (OPedidoEntrega.id > 0);   
		}
	}
}