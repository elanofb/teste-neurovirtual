using System.Collections.Specialized;
using System.Linq;
using DAL.Associados;

namespace BLL.NaoAssociados {

	public interface INaoAssociadoBL {

	    IQueryable<Associado> query(int? idOrganizacaoParam = null);

		IQueryable<Associado> carregar(int idAssociado);

	    Associado carregarPorPessoa(int idPessoa);

        IQueryable<Associado> listar(string valorBusca, string ativo);

        bool existe(int idTipoDocumento, string cpf, string email, string login, int idDesconsiderado);

        bool excluir(int idAssociado, string motivo);
	}
}
