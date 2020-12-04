using System;
using System.Data.Entity;
using System.Linq;
using BLL.Core.Events;
using BLL.Services;
using DAL.Pedidos;
using DAL.Pessoas;

namespace BLL.Pedidos {

    public class PedidoRelatorioGeralBL : DefaultBL {

        // Events
        private EventAggregator onPedidoCadastro => OnPedidoCadastrado.getInstance;

        //
        public PedidoRelatorioGeralBL() {

        }

        //
        public IQueryable<Pedido> query(int? idOrganizacaoParam = null) {

            var query = from Obj in this.db.Pedido
                where Obj.flagExcluido == "N"
                select Obj;

            if (idOrganizacaoParam == null) {
                idOrganizacaoParam = idOrganizacao;
            }

            if (idOrganizacaoParam > 0) {
                query = query.Where(x => x.idOrganizacao == idOrganizacaoParam);
            }

            return query;

        }

    }
}