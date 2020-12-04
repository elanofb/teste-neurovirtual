using System.Linq;
using DAL.Pessoas;

namespace BLL.Associados {

	public interface IAssociadoRelacionamentoBL {

	    PessoaRelacionamento carregar(int id);
	    
        IQueryable<PessoaRelacionamento> listar(int idPessoa, int idOcorrenciaRelacionamento);

	    bool salvar(PessoaRelacionamento OPessoaRelacionamento);

	    bool registrarCadastro(int idPessoa);

	    bool excluir(int[] ids);

	}
}
