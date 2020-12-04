using System.Linq;
using DAL.PedidosTemp;

namespace BLL.PedidosTemp {

    public interface IPedidoProdutoTempBL {
        
        IQueryable<PedidoProdutoTemp> query();
        
        PedidoProdutoTemp carregar(int id);
        
        bool salvar(PedidoProdutoTemp OPedidoProdutoTemp);

        void excluir(int id);

    }

}
