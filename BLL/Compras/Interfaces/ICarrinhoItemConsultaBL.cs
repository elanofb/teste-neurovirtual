using System.Linq;
using DAL.Compras;

namespace BLL.Compras {

    public interface ICarrinhoItemConsultaBL {

        IQueryable<CarrinhoItem> listar(int idOrganizacaoParam, int idPessoa, string idSessao, bool? flagComprado = false);

        IQueryable<CarrinhoItemProdutoVW> listarResumo(int idOrganizacaoParam, int idPessoa, string idSessao, bool? flagComprado = false);
    }
}