using System.Linq;
using DAL.Documentos;

namespace BLL.Documentos {

	public interface ITipoDocumentoBL{


		TipoDocumento carregar(int id);
		IQueryable<TipoDocumento> listar(int idCategoriaDocumento, string valorBusca, string ativo);
		bool salvar(TipoDocumento OTipoDocumento);
		bool excluir(int id);
	}
}