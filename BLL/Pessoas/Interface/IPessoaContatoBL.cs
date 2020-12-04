using System;
using System.Linq;
using DAL.Pessoas;

namespace BLL.Pessoas {

	public interface IPessoaContatoBL{
		PessoaContato carregar(int id);
		IQueryable<PessoaContato> listar(int idPessoa, int idTipoContato, string ativo);
		bool salvar(PessoaContato OContato);
		bool existe(string nome, string email, int idPessoa, int idDesconsiderar);
		UtilRetorno excluir(int id);
	}
}