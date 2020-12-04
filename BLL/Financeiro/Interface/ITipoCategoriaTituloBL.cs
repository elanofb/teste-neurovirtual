using System.Linq;
using DAL.Financeiro;
using System.Json;
using UTIL.Resources;

namespace BLL.Financeiro {

	public interface ITipoCategoriaTituloBL {
        TipoCategoriaTitulo carregar(int id);
        IQueryable<TipoCategoriaTitulo> listar(int idMacroConta, int idCategoria, string valorBusca,string ativo);
        bool existe(int? idCategoria, string descricao,int id);
        bool salvar(TipoCategoriaTitulo OTipoProduto);
        bool excluir(int id);
        JsonMessageStatus alterarStatus(int id);
        IQueryable<TipoCategoriaTitulo> autocompletar(int idCategoria); 

	}
}
