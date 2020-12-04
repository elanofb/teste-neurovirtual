using System.Linq;
using DAL.Pedidos;

namespace BLL.Pedidos {

    public interface IPedidoProdutoRendimentoConsultaBL {
        /// <summary>
        /// 
        /// </summary>
        IQueryable<PedidoProdutoRendimento> query();
    }

}