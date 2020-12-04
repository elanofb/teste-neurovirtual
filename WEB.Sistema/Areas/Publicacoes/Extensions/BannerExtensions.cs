using System;
using System.Web.Mvc;
using DAL.Publicacoes;
using System.IO;

namespace WEB.Areas.Publicacoes.Extensions {

    public static class BannerExtensions{
		
		//
        public static MvcHtmlString pathImagem(this Banner OBanner, bool flagThumb = false){
			string pathImagemPadrao = "default/sem-imagem.gif";

			if (OBanner.Arquivo == null) {
				return new MvcHtmlString(pathImagemPadrao);
			}

            string pathImagem = OBanner.Arquivo.path;

            if (flagThumb) {
                pathImagem = Path.Combine(UtilString.notNull(OBanner.Arquivo.pathThumb), "sistema", UtilString.notNull(OBanner.Arquivo.nomeArquivo));
            }

			if (!File.Exists(Path.Combine(UtilConfig.pathAbsUploadFiles, pathImagem))) {
				return new MvcHtmlString(pathImagemPadrao);
			}

			return new MvcHtmlString(pathImagem);
        }

    }
}