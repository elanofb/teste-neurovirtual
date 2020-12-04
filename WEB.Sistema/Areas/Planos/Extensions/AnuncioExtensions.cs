using System;
using System.Web.Mvc;
using System.IO;

namespace WEB.Areas.Planos.Extensions {

    public static class AnuncioExtensions{
		
		//
        public static MvcHtmlString pathImagem(this Anuncio OAnuncio, bool flagThumb = false){
			string pathImagemPadrao = "default/sem-imagem.gif";

			if (OAnuncio.Foto == null) {
				return new MvcHtmlString(pathImagemPadrao);
			}

            string pathImagem = OAnuncio.Foto.path;

            if (flagThumb) {
                pathImagem = Path.Combine(UtilString.notNull(OAnuncio.Foto.pathThumb), "sistema", UtilString.notNull(OAnuncio.Foto.nomeArquivo));
            }

			if (!File.Exists(Path.Combine(UtilConfig.pathAbsUploadFiles, pathImagem))) {
				return new MvcHtmlString(pathImagemPadrao);
			}

			return new MvcHtmlString(pathImagem);
        }

    }
}