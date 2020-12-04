using System.Linq;
using DAL.Associados;

namespace BLL.AssociadosInstitucional {

	public interface IAssociadoInstitucionalBL {

		Associado carregar(int idAssociado);

		Associado carregarAssociadoPessoa(int idPessoa);

		IQueryable<Associado> listar(int idTipoAssociado, string flagSituacao, string valorBusca, string ativo);

		Associado salvar(Associado OAssociado);

		bool existe(int idTipoDocumento, string cpf, string email, string login, int idDesconsiderado);
	}
}
