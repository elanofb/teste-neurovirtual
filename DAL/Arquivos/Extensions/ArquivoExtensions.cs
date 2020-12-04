using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DAL.Arquivos.Extensions {

	public static class ArquivoExtensions {

        public static string pathImagemPadrao = "default/sem-imagem.png";

		//Carregar o caminho onde a imagem foi salva
		public static string pathImagem(this ArquivoUpload OArquivo, string localThumb = "") {


			if (OArquivo == null) {
				return (pathImagemPadrao);
			}

			string pathImagem = OArquivo.path.stringOrEmpty();

			if (!String.IsNullOrEmpty(localThumb)) {
				pathImagem = String.Concat(UtilString.notNull(OArquivo.pathThumb), localThumb, "/", UtilString.notNull(OArquivo.nomeArquivo));
			}

		    string pathUpload = OArquivo.idOrganizacao.toInt() == 0? UtilConfig.pathAbsUploadFiles : UtilConfig.pathAbsUpload(OArquivo.idOrganizacao.toInt());

			if (!File.Exists(Path.Combine(pathUpload, pathImagem))) {
				return pathImagemPadrao;
			}

			return pathImagem;
		}

		//Link completo da imagem
		public static string linkArquivo(this ArquivoUpload OArquivo) {

		    int idOrganizacao = OArquivo.idOrganizacao.toInt();

		    string basePath = idOrganizacao > 0 ? UtilConfig.linkAbsSistemaUpload(idOrganizacao) : $"{UtilConfig.linkAbsSistema}upload/";
            
		    return String.Concat(basePath, OArquivo.path);
		}

		//Link completo da imagem
		public static string linkImagem(this ArquivoUpload OArquivo, string localThumb = ""){

		    if (OArquivo == null || OArquivo.id == 0){

		        return $"{UtilConfig.linkAbsSistema}upload/{pathImagemPadrao}";
		    }

		    int idOrganizacao = OArquivo.idOrganizacao.toInt();

		    string basePath = idOrganizacao > 0 ? UtilConfig.linkAbsSistemaUpload(idOrganizacao) : $"{UtilConfig.linkAbsSistema}upload/";

		    string pathImg = OArquivo.pathImagem(localThumb).Replace("\\", "/");

		    if (pathImg.Contains("sem-imagem")){

		        return $"{UtilConfig.linkAbsSistema}upload/{pathImg}";
		    }

			return String.Concat(basePath, pathImg);
		}

        //Link completo do áudio
		public static string linkAudio(this ArquivoUpload OArquivo) {

			return String.Concat(UtilConfig.linkAbsSistema, "upload/", OArquivo.pathImagem("").Replace("\\", "/"));

		}

		//Link completo da imagem
		public static string linkFisico(this ArquivoUpload OArquivo, string localThumb = "") {

			int idOrganizacao = OArquivo.idOrganizacao.toInt();
			
			string basePath = idOrganizacao > 0 ? UtilConfig.pathAbsUpload(idOrganizacao) : $"{UtilConfig.pathAbsRaiz}upload/";
			
			return String.Concat(basePath, OArquivo.pathImagem(localThumb).Replace("\\", "/"));

		}

		//Link completo da imagem
		public static string linkFirstImage(this IList<ArquivoUpload> listaArquivo, string localThumb = "") {

			var OArquivo = listaArquivo.OrderByDescending(x => x.id).FirstOrDefault();

			return String.Concat(UtilConfig.linkAbsSistema, "upload/", OArquivo.pathImagem(localThumb).Replace("\\", "/"));

		}
		
		//Link completo da imagem
		public static FileInfo toFileInfo(this ArquivoUpload OArquivo) {

			if (OArquivo == null) {
				return null;
			}
			
			int idOrganizacao = OArquivo.idOrganizacao.toInt();

			string basePath = UtilConfig.pathAbsUpload(idOrganizacao);
			
			string pathFile = Path.Combine(basePath, OArquivo.path.Replace("\\", "/"));

			var OFileInfo = new FileInfo(pathFile);

			return OFileInfo;

		}
	}
}
