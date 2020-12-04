using System.Linq;
using DAL.Financeiro;

namespace BLL.Financeiro {

	public interface IMeioPagamentoBL {
        MeioPagamento carregar(int id);
        IQueryable<MeioPagamento> listar(string valorBusca,string ativo);
        bool existe(string descricao,int id);
        bool salvar(MeioPagamento OMeioPagamento);
        bool excluir(int id);
	}
}
