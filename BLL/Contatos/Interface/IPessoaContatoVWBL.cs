using System.Linq;
using DAL.Contatos;
using System.Collections.Generic;

namespace BLL.Contatos {

	public interface IPessoaContatoVWBL {

        PessoaContatoVW carregar(int idPessoa);

        IQueryable<PessoaContatoVW> listar(string ativo, string flagSituacaoContribuicao, List<int> idsTipoContato, List<int> idsTipoAssociado, string valorBusca);

	}
}
