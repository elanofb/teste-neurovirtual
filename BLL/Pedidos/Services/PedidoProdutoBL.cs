using System;
using System.Linq;
using BLL.Core.Events;
using BLL.Services;
using DAL.Pedidos;
using EntityFramework.Extensions;

namespace BLL.Pedidos {

	public class PedidoProdutoBL : DefaultBL, IPedidoProdutoBL {

        // Events
	    private EventAggregator onProdutoAdicionado => OnProdutoAdicionado.getInstance;

		//
		public PedidoProdutoBL() {

		}		
		
	    //
	    public IQueryable<PedidoProduto> query() {
			
	        var query = from Obj in db.PedidoProduto
	            where Obj.flagExcluido == "N"
	            select Obj;
            
	        return query;

	    }
		
	    //
	    public PedidoProduto carregar(int id) {
            
	        var query = this.query().Where(x => x.id == id);

	        query = query.condicoesSeguranca();

	        return query.FirstOrDefault();

	    }

		//
		public IQueryable<PedidoProduto> listar(int idPedido) {

			var query = this.query();

			if (idPedido > 0) {
				query = query.Where(x => x.idPedido == idPedido);
			}

			return query;
		}

		//
		public bool salvar(PedidoProduto OPedidoProduto) {

		    OPedidoProduto.Pedido = null;

		    OPedidoProduto.Produto = null;
            
			if (OPedidoProduto.id == 0) {
                return this.inserir(OPedidoProduto);
            }

		    return this.atualizar(OPedidoProduto);

		}

        //
        private bool inserir(PedidoProduto OPedidoProduto) {

            OPedidoProduto.setDefaultInsertValues();

            db.PedidoProduto.Add(OPedidoProduto);

            db.SaveChanges();
            
            return OPedidoProduto.id > 0;

        }

	    //
	    private bool atualizar(PedidoProduto OPedidoProduto) {

	        //Localizar existentes no banco
	        var dbPedidoProduto = this.carregar(OPedidoProduto.id);

	        if (dbPedidoProduto == null) {
	            return false;
	        }

	        var dbEntry = db.Entry(dbPedidoProduto);

	        OPedidoProduto.setDefaultUpdateValues();

	        dbEntry.CurrentValues.SetValues(OPedidoProduto);

	        dbEntry.ignoreFields(new [] { "idPedido", "idProduto" });

	        db.SaveChanges();

	        return (OPedidoProduto.id > 0);

	    }

		
	}
}