using System;
using System.IO;
using System.Json;
using System.Web;
using System.Web.Mvc;
using DAL.Permissao.Security.Extensions;
using UTIL.Upload;

namespace WEB.Areas.Arquivos.Controllers {
	public class FroalaUploadController : Controller {

		//Constantes

		//Atributos


		//Salvar uma foto do editor
		[HttpPost, ActionName("salvar-foto")]
		public ActionResult salvarFoto(HttpPostedFileBase file) {

			var Retorno = this.uploadImagemFroala(file);

			return Json( Retorno, JsonRequestBehavior.AllowGet);
		}

		//Salvar um arquivo do editor
		[HttpPost, ActionName("salvar-arquivo")]
		public ActionResult salvarArquivo(HttpPostedFileBase file) {

			var Retorno = this.uploadArquivoFroala(file);

			return Json( Retorno, JsonRequestBehavior.AllowGet);
		}

		//Salvar a imagem para o plugin FROALA EDITOR
		private object uploadImagemFroala(HttpPostedFileBase OFile) {

			if (!UploadFileValidation.isImageExtension(OFile)) {
				return new JsonMessage{ error = true, message = "A extensão da imagem não é válida."};
			}
			
			if (!UploadFileValidation.isImageType(OFile.InputStream)) {
				return new JsonMessage{ error = true, message = "O tipo do arquivo é inválido."};
			}

		    int idOrganizacao = User.idOrganizacao();

			string pathUpload = Path.Combine(UtilConfig.pathAbsUpload(idOrganizacao), "froala");
			string extensao = UploadFileValidation.getExtension(OFile);
			string fileName = String.Concat(UtilString.onlyNumber(DateTime.Now.ToString()), Guid.NewGuid(), extensao);
			string fullPathFile = Path.Combine(pathUpload, fileName);

			if (!Directory.Exists(pathUpload)) {
				UtilIO.createFolder(pathUpload);
			}

			OFile.SaveAs(fullPathFile);
			
			string linkImagem = String.Concat(UtilConfig.linkAbsSistemaUpload(idOrganizacao), "froala/", fileName);

          	return new { link = linkImagem};
		}

		//Salvar o arquivo para o plugin FROALA EDITOR
		private object uploadArquivoFroala(HttpPostedFileBase OFile) {

			if (!UploadFileValidation.isAllowedExtension(OFile)) {
				return new JsonMessage{ error = true, message = "A extensão do arquivo não é válida."};
			}

            int idOrganizacao = User.idOrganizacao();

			string pathUpload = Path.Combine(UtilConfig.pathAbsUpload(idOrganizacao), "froala");
			string extensao = UploadFileValidation.getExtension(OFile);
			string fileName = String.Concat(UtilString.onlyNumber(DateTime.Now.ToString()), Guid.NewGuid(), extensao);
			string fullPathFile = Path.Combine(pathUpload, fileName);

			if (!Directory.Exists(pathUpload)) {
				UtilIO.createFolder(pathUpload);
			}

			OFile.SaveAs(fullPathFile);
			
			string linkArquivo = String.Concat(UtilConfig.linkAbsSistemaUpload(idOrganizacao), "froala/", fileName);

          	return new { link = linkArquivo};
		}
	}
}