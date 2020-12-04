using System.Json;
using DAL.DocumentosDigitais;
using System.Linq;

namespace BLL.DocumentosDigitais {

    public interface IDocumentoDigitalBL {

        IQueryable<DocumentoDigital> query(int? idOrganizacaoParam = null);

		DocumentoDigital carregar(int id);

		IQueryable<DocumentoDigital> listar(string valorBusca, int idTipoDocumento, string flagTipoPessoa, bool? ativo);

        bool salvar(DocumentoDigital ODocumentoDigital);

        JsonMessageStatus alterarStatus(int id);

        bool excluir(int id);

	}
}