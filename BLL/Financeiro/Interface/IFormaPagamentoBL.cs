using System.Linq;
using DAL.Financeiro;

namespace BLL.Financeiro {

	public interface IFormaPagamentoBL {
        FormaPagamento carregar(int id);
        IQueryable<FormaPagamento> listar(string valorBusca,string ativo);
        bool existe(string descricao,int id);
        bool salvar(FormaPagamento OTipoProduto);
        bool excluir(int id);
	}
}
