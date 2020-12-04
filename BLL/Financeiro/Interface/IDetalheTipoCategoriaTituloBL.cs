using System.Linq;
using DAL.Financeiro;
using System.Json;
using UTIL.Resources;

namespace BLL.Financeiro {

	public interface IDetalheTipoCategoriaTituloBL {
        DetalheTipoCategoriaTitulo carregar(int id);
        IQueryable<DetalheTipoCategoriaTitulo> listar(int idCentroCusto, int idCategoria, int idTipoCategoria, string valorBusca,string ativo);
        bool existe(int? idTipoCategoria, string descricao,int id);
        bool salvar(DetalheTipoCategoriaTitulo OTipoProduto);
        bool excluir(int id);
        JsonMessageStatus alterarStatus(int id);
        IQueryable<DetalheTipoCategoriaTitulo> autocompletar(int idTipoCategoria); 
	}
}
