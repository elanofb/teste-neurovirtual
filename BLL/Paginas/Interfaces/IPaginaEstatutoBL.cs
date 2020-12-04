using System.Linq;
using DAL.Paginas;

namespace BLL.Paginas {

	public interface IPaginaEstatutoBL {

        PaginaEstatuto carregar(int idOrganizacaoParam = 0);
        
		IQueryable<PaginaEstatuto> listar(int idOrganizacao);

		bool salvar(PaginaEstatuto OPaginaEstatuto);

	}
}