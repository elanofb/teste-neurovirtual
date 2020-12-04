using System;
using System.Linq;
using System.Web;
using DAL.Arquivos;
using System.Collections.Generic;
using System.Json;

namespace BLL.Arquivos {

	public interface IArquivoUploadBL {

	    ArquivoUpload carregar(int id);

		ArquivoUpload carregar(int idReferencia, string categoria, string entidade);
		
		IQueryable<ArquivoUpload> listar(int idReferencia, string entidade, string categoria, string ativo);
        
		IQueryable<ArquivoUpload> listarDocumentos(int idReferencia, string entidade);

        IQueryable<ArquivoUpload> listarAudios(int idReferencia, string entidade);

		string salvar(ArquivoUpload OArquivo, HttpPostedFileBase FileUpload, string pathUpload = "");

		bool salvarLogotipo(int idReferencia, string entidade, HttpPostedFileBase Logotipo, List<ThumbDTO> listaThumb = null);
        
		bool salvarDocumento(int idReferencia, string entidade, string descricao, HttpPostedFileBase Documento, int idOrganizacaoParam, int idUsuarioCadastroParam = 0);

        bool salvarAudio(int idReferencia, string entidade, string descricao, HttpPostedFileBase Audio);

		bool upload(ref ArquivoUpload OArquivo, HttpPostedFileBase FileUpload, string pathUpload = "", List<ThumbDTO> listaThumb = null);

		void atualizarDados(int idArquivo, string nomeCampo, string novoValor);

		ArquivoUpload alterarStatus(int id);

		bool excluir(int id);

		UtilRetorno excluir(int idReferencia, string entidade);

	    JsonMessage salvarOrder(int[] ids);

	}
}
