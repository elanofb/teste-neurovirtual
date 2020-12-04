using System.Linq;
using DAL.Associados;

namespace BLL.NaoAssociadosInstitucional {

	public interface INaoAssociadoInstitucionalBL {

		Associado carregar(int idNaoAssociado);

		Associado carregarAssociadoPessoa(int idPessoa);

		IQueryable<Associado> listar(int idTipoAssociado, string flagSituacao, string valorBusca, string ativo);

		Associado salvar(Associado ONaoAssociado);

		bool existe(int idTipoDocumento, string cpf, string email, string login, int idDesconsiderado);
	}
}
