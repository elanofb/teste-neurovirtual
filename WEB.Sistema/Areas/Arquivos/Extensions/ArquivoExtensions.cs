using WEB.Areas.Imagem.Helpers;
using System.Drawing;
using System.Web.Helpers;
using System.IO;
using System.Web.Mvc;
using System.Text;
using DAL.Arquivos;
using System;

namespace WEB.Areas.Arquivos.Extensions{

    public static class ArquivoExtensions{

		//Constantes
		private static string pathImgPadraoThumb = "Areas/Arquivos/img/sem_foto_100.jpg";
		private static string pathImgPadrao = "Areas/Arquivos/img/sem_foto.jpg";

		//Recuperar o caminho de um arquivo a partir da pasta "upload", considerando thumb
		public static string srcImgThumb(this ArquivoUpload OArquivo, string subPasta = "sistema") {

            if (OArquivo == null){

		        return pathImgPadrao;
		    }

		    int idOrganizacao = OArquivo.idOrganizacao.toInt();

		    string basePath = idOrganizacao > 0 ? UtilConfig.pathAbsUpload(idOrganizacao) : UtilConfig.pathAbsUploadFiles;

            string fullPath = Path.Combine(basePath, UtilString.notNull(OArquivo.pathThumb), subPasta, OArquivo.nomeArquivo.stringOrEmpty());
			
			if (!File.Exists(fullPath)) { 

				return pathImgPadraoThumb;

			}
			
            return $"upload/{UtilConfig.pathOrganizacao(idOrganizacao)}/{UtilString.notNull(OArquivo.pathThumb)}/{subPasta}/{OArquivo.nomeArquivo.stringOrEmpty()}";
		}

		//Recuperar o caminho de um arquivo a partir da pasta "upload"
		public static string srcImg(this ArquivoUpload OArquivo) {

		    if (OArquivo == null){

		        return pathImgPadrao;
		    }

		    int idOrganizacao = OArquivo.idOrganizacao.toInt();

		    string basePath = idOrganizacao > 0 ? UtilConfig.pathAbsUpload(idOrganizacao) : UtilConfig.pathAbsUploadFiles;

			string fullPath = Path.Combine(basePath, UtilString.notNull(OArquivo.path));
			
			if (!File.Exists(fullPath)) { 

				return pathImgPadrao;

			}

			return $"upload/{UtilConfig.pathOrganizacao(idOrganizacao)}/{OArquivo.path}";
		}

		//
		private static string getSufixoTamanho(int novaLargura, int novaAltura) {
			string sufixoTamanho = String.Concat("_", novaLargura, "_", novaAltura);
			return sufixoTamanho;
		}

        //
		public static string capturarImagem(this ArquivoUpload OArquivoUpload, int novaLargura = 0, int novaAltura = 0) { 
			
			string imagemOriginal = OArquivoUpload.pathImagemOriginal(novaLargura, novaAltura);
			if (File.Exists(imagemOriginal)) { 
				return imagemOriginal;
			}

			string imagemPadrao = pathImagemPadrao(novaLargura, novaAltura);
			return imagemPadrao;
		}


        //Retornar o path da imagem Original
        private static string pathImagemOriginal(this ArquivoUpload OArquivoUpload, int novaLargura = 0, int novaAltura = 0) {
			string pathImagemOriginal = "";
			string pathImagemNoTamanho = "";
			string sufixoTamanho = getSufixoTamanho(novaLargura, novaAltura);

			if (OArquivoUpload == null) { 
				return "";
			}

			pathImagemOriginal = Path.Combine(UtilConfig.pathAbsUploadFiles, OArquivoUpload.path);
			pathImagemNoTamanho = pathImagemOriginal.Replace(OArquivoUpload.extensao, String.Concat(sufixoTamanho, OArquivoUpload.extensao));

			if (File.Exists(pathImagemNoTamanho)) { 
				return pathImagemNoTamanho;
			}

			if (!File.Exists(pathImagemOriginal)) { 
				return "";
			}

			if(novaAltura == 0 && novaAltura == 0){
                return pathImagemOriginal;
			}

			return redimensionar(pathImagemOriginal, pathImagemNoTamanho, novaLargura, novaAltura);
        }


		//
        private static string pathImagemPadrao(int novaLargura = 0, int novaAltura = 0) {
			string pathImagem = HttpContextFactory.Current.Server.MapPath(ImagemHelper.pathImgPadrao);
			string pathImagemNoTamanho = "";
			string sufixoTamanho = getSufixoTamanho(novaLargura, novaAltura);

			pathImagemNoTamanho = pathImagem.Replace(".jpg", String.Concat(sufixoTamanho, ".jpg"));

			if (File.Exists(pathImagemNoTamanho)) { 
				return pathImagemNoTamanho;
			}

			if(novaLargura == 0 && novaAltura == 0){
                return pathImagem;
			}

			return redimensionar(pathImagem, pathImagemNoTamanho, novaLargura, novaAltura);

		}

		//
		private static string redimensionar(string pathImagemOriginal, string pathImagemNoTamanho, int novaLargura = 0, int novaAltura = 0){
			var Imagem = Image.FromFile(pathImagemOriginal);
			if (novaAltura == 0) { 
				novaAltura = (int)Math.Round(Imagem.Height * (decimal)novaLargura / Imagem.Width, 0);
			}

			if (novaLargura == 0) { 
				novaLargura = (int)Math.Round(Imagem.Width * (decimal)novaAltura / Imagem.Height, 0);
			}
			
			WebImage OWebImage = new WebImage(pathImagemOriginal);
			
			OWebImage.Resize(novaLargura, novaAltura, true, true)
					 .Save(pathImagemNoTamanho);

			return pathImagemNoTamanho;

		} 

		//Exibir ícones
        public static MvcHtmlString exibirIcone(this UrlHelper helper, string extensao) { 

			string basePath = "~/Areas/Arquivos/img/";
            string pathIcone = helper.Content(String.Concat(basePath, "sem_foto_100.jpg"));

			if(extensao == ".mp3" || extensao == ".mp4") { 

				pathIcone =  helper.Content(String.Concat(basePath, "icons/64/audio.png"));

			}else if(extensao == ".pdf"){

				pathIcone =  helper.Content(String.Concat(basePath, "icons/64/pdf.png"));

			}else if(extensao == ".xls" || extensao == ".xlsx"){

				pathIcone =  helper.Content(String.Concat(basePath, "icons/64/excel.png"));

			}else if(extensao == ".doc" || extensao == ".docx"){

				pathIcone =  helper.Content(String.Concat(basePath, "icons/64/word.png"));

			}else if(extensao == ".zip" || extensao == ".rar"){

				pathIcone =  helper.Content(String.Concat(basePath, "icons/64/zip.png"));

			}else if(extensao == ".ppt" || extensao == ".pptx" || extensao == ".pps" || extensao == ".ppsx"){

				pathIcone =  helper.Content(String.Concat(basePath, "icons/64/ppt.png"));

			}else if(extensao == ".txt" || extensao == ".sql"){

				pathIcone =  helper.Content(String.Concat(basePath, "icons/64/text.png"));

			}

            StringBuilder html = new StringBuilder();
            html.Append("<img src=\"" + pathIcone + "\"  height=\"50\" />");

            return new MvcHtmlString(html.ToString());
        }

    }
}