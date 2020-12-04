using System.Json;
using DAL.Arquivos;

namespace BLL.Arquivos {

	public interface IArquivoUploadFotoBL : IArquivoUploadPadraoBL {

        ArquivoUpload carregarPrincipal(int idReferencia, string entidade, int? idOrganizacaoParam = null);
 
	    JsonMessageStatus registrarFotoPrincipal(int id);
        
	    void reordenarExibicao(int id, byte ordem);

	}
}