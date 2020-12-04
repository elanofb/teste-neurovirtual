using System.Linq;
using DAL.Financeiro;
using System.Json;
using UTIL.Resources;

namespace BLL.Financeiro {

	public interface ICategoriaTituloBL {
        CategoriaTitulo carregar(int id);
        IQueryable<CategoriaTitulo> listar(int idMacroConta, string valorBusca,string ativo);
        bool existe(int? idMacroConta, string descricao,int id);
        bool salvar(CategoriaTitulo OTipoProduto);
        bool excluir(int id);
        JsonMessageStatus alterarStatus(int id);
        IQueryable<CategoriaTitulo> autocompletar(int idMacroConta); 
	}
}
