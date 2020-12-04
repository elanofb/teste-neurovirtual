using System;
using System.Linq;
using System.Web.Mvc;
using BLL.Arquivos;
using DAL.Arquivos;
using DAL.Entities;
using DAL.Permissao.Security.Extensions;
using DevTrends.MvcDonutCaching;
using WEB.Areas.Arquivos.Extensions;

namespace WEB.Areas.Arquivos.Controllers {

	public class ExibicaoController : Controller {

		//Constantes

	    //Atributos
	    private IArquivoUploadBL _ArquivoUploadBL; 
	    private IArquivoUploadFotoBL _IArquivoUploadFotoBL; 
	
	    //Propriedades
	    private IArquivoUploadBL OArquivoUploadBL => _ArquivoUploadBL = _ArquivoUploadBL ?? new ArquivoUploadBL();
	    private IArquivoUploadFotoBL OArquivoUploadFotoBL => _IArquivoUploadFotoBL = _IArquivoUploadFotoBL ?? new ArquivoUploadFotoBL(); 
		
		//Eventos

		//
		public FileResult index(int idNum, string idCrypt) {

			var idCryptCompare = UtilCrypt.SHA512(idNum.ToString());
			if (idCryptCompare != idCrypt) {
				Response.Write("A Url informada não é válida.");
				return null;
			}

			if (User.idOrganizacao() == 0) {
				Response.Write("Você não tem permissão para acessar essa página.");
				return null;
			}
			
			var OArquivo = this.OArquivoUploadBL.carregar(idNum);
			if (OArquivo == null) {
				Response.Write("O arquivo não foi encontrado.");
                return null;
			}

			if (OArquivo.idOrganizacao != User.idOrganizacao()) {
				Response.Write("Você não tem permissão para acessar essa página.");
			}

		    int idOrganizacao = OArquivo.idOrganizacao.toInt();

		    string basePath = idOrganizacao > 0 ? UtilConfig.pathAbsUpload(idOrganizacao) : $"{UtilConfig.pathAbsUploadFiles}upload/";

		    string pathArquivo = OArquivo.path.Replace("\\", "/");

			string pathCompletoArquivo = String.Concat(basePath, pathArquivo);

			if (!System.IO.File.Exists(pathCompletoArquivo)) {
				Response.Write("O arquivo não foi encontrado.");
                return null;
			}

			string nomeDownload = string.Concat((OArquivo.titulo.isEmpty() ? "download" : OArquivo.titulo), OArquivo.extensao);

			return File(pathCompletoArquivo, OArquivo.contentType, nomeDownload);
		}

		//
		[ActionName("exibir-arquivo")]
		public FileResult exibirArquivo(int idArquivo) {
			
			ArquivoUpload OArquivo = this.OArquivoUploadBL.carregar(idArquivo);

			if (OArquivo == null) {

				Response.Write("O arquivo não foi encontrado.");

                return null;
			}

		    int idOrganizacao = OArquivo.idOrganizacao.toInt();

		    string basePath = idOrganizacao > 0 ? UtilConfig.pathAbsUpload(idOrganizacao) : $"{UtilConfig.pathAbsUploadFiles}upload/";

		    string pathArquivo = OArquivo.path.Replace("\\", "/");

			string pathCompletoArquivo = String.Concat(basePath, pathArquivo);

			if (!System.IO.File.Exists(pathCompletoArquivo)) {

				Response.Write("O arquivo não foi encontrado.");

                return null;
			}

			string nomeDownload = String.Concat( (String.IsNullOrEmpty(OArquivo.titulo) ? "download" : OArquivo.titulo), OArquivo.extensao);

			return File(pathCompletoArquivo, OArquivo.contentType, nomeDownload);
		}

        //
		[ActionName("exibir-logotipo-atual"), DonutOutputCache(Duration=600), AllowAnonymous]
        public ActionResult exibirLogotipoAtual(int largura = 0, int altura = 0) {
			
            var OArquivo = this.OArquivoUploadFotoBL.listar(0, EntityTypes.LOGOTIPO, "S").OrderByDescending(x => x.id).FirstOrDefault();


			string pathImagem = OArquivo.capturarImagem(largura, altura);

			string relativePath = pathImagem.Replace(Request.ServerVariables["APPL_PHYSICAL_PATH"], String.Empty);

			//Response.Write(relativePath);

			return File(pathImagem, "image/jpeg" );

			//return relativePath;
        }
		
		
		
		//
		[ActionName("exibir-arquivo-crypt")]
		public FileResult exibirArquivoCrypt(string id) {
			
			var idArquivo = UtilNumber.toInt32(UtilCrypt.toBase64Decode(id));
			ArquivoUpload OArquivo = this.OArquivoUploadBL.carregar(idArquivo);

			if (OArquivo == null) {

				Response.Write("O arquivo não foi encontrado.");

				return null;
			}

			int idOrganizacao = OArquivo.idOrganizacao.toInt();

			string basePath = idOrganizacao > 0 ? UtilConfig.pathAbsUpload(idOrganizacao) : $"{UtilConfig.pathAbsUploadFiles}upload/";

			string pathArquivo = OArquivo.path.Replace("\\", "/");

			string pathCompletoArquivo = String.Concat(basePath, pathArquivo);

			if (!System.IO.File.Exists(pathCompletoArquivo)) {

				Response.Write("O arquivo não foi encontrado.");

				return null;
			}

			string nomeDownload = String.Concat( (String.IsNullOrEmpty(OArquivo.titulo) ? "download" : OArquivo.titulo), OArquivo.extensao);

			return File(pathCompletoArquivo, OArquivo.contentType, nomeDownload);
		}
	}
}