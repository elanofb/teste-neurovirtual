using DAL.Compras;

namespace BLL.Compras {

	public interface ICarrinhoItemBL {
		
		CarrinhoItem carregarExistente(int idItem, int? idPessoa, string idSessao);

		void remover(int id);

		void limpar(int? idPessoa, string idSessao);

		void vincular(int? idPessoaLogada, string idSessao);
	}
}
