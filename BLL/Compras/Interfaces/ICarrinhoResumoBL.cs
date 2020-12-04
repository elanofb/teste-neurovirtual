using System;
using DAL.Compras;

namespace BLL.Compras {

	public interface ICarrinhoResumoBL {
		
		CarrinhoResumo carregarExistente(int idOrganizacaoParam, int? idPessoa, string idSessao);

		void salvar(CarrinhoResumo OCarrinhoResumo);

		void limpar(int idOrganizacaoParam, int? idPessoa, string idSessao);

	    UtilRetorno limparFrete(int idOrganizacaoParam, int? idPessoaLogada, string idSessao);

		void vincular(int? idPessoaLogada, string idSessao);
	}
}
