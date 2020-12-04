using System.Linq;
using BLL.Services;
using DAL.Pedidos;

namespace BLL.Pedidos {

    /// <summary>
    /// 
    /// </summary>
    public class PedidoProdutoRendimentoConsultaBL: DefaultBL, IPedidoProdutoRendimentoConsultaBL {
        
        
        /// <summary>
        /// 
        /// </summary>
        public IQueryable<PedidoProdutoRendimento> query() {

            var query = from Obj in this.db.PedidoProdutoRendimento
                        select Obj;

            return query;

        }        
    }

}
