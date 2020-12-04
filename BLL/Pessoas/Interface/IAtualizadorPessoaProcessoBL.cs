using DAL.Pessoas;
using DAL.ProcessosAvaliacoes;

namespace BLL.Pessoas {

	public interface IAtualizadorPessoaProcessoBL {

		void atualizar(int idPessoa, object OrigemDados);

		void atualizarEnderecos(Pessoa OPessoa, ProcessoAvaliacaoInscricao OProcessoAvaliacaoInscricao);

		void atualizarEmails(Pessoa OPessoa, ProcessoAvaliacaoInscricao OProcessoAvaliacaoInscricao);

		void atualizarTelefones(Pessoa OPessoa, ProcessoAvaliacaoInscricao OProcessoAvaliacaoInscricao);

	}
}