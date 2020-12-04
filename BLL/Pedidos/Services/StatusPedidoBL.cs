using System;
using System.Linq;
using DAL.Repository.Base;
using DAL.Pedidos;

namespace BLL.Pedidos {

	public class StatusPedidoBL : TableRepository<StatusPedido>, IStatusPedidoBL {

		//
		public StatusPedidoBL() {
		}

		//
		public IQueryable<StatusPedido> listar(string valorBusca, string ativo) {
			var db = this.getDataContext();
			var query = from T in db.StatusPedido
						where T.flagExcluido == "N"
						select T;

			if (!String.IsNullOrEmpty(valorBusca)) {
				query = query.Where(x => x.descricao.Contains(valorBusca));
			}

			if (!String.IsNullOrEmpty(ativo)) {
				query = query.Where(x => x.ativo == ativo);
			}

			return query;
		}
	}
}