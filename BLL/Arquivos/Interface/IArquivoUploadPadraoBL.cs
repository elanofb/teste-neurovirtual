using System;
using System.Collections.Generic;
using System.Json;
using System.Linq;
using System.Web;
using DAL.Arquivos;

namespace BLL.Arquivos {

	public interface IArquivoUploadPadraoBL {
        
	    IQueryable<ArquivoUpload> query(bool flagFiltroCategoria = true);

		ArquivoUpload carregar(int id);
        
	    IQueryable<ArquivoUpload> listar(int idReferencia, string entidade, string ativo, int? idOrganizacaoParam = null);

		bool salvar(ArquivoUpload OArquivo, HttpPostedFileBase FileUpload = null, string pathUpload = "", List<ThumbDTO> listaThumb = null);
        
		void atualizarDados(int idArquivo, string nomeCampo, string novoValor);
		
	    JsonMessageStatus alterarStatus(int id);
        
		UtilRetorno excluir(int idReferencia, string entidade);
        
		JsonMessageStatus excluir(int id);
        
	}

}