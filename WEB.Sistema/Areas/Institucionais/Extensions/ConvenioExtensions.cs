using System;
using System.Web.Mvc;
using DAL.Publicacoes;
using System.IO;

namespace WEB.Areas.Convenios.Extensions {

    public static class ConvenioExtensions {

        //
        public static MvcHtmlString pathImagem(this Noticia ONoticia, bool flagThumb = false) {
            string pathImagemPadrao = "default/sem-imagem.gif";

            if (ONoticia.Foto == null) {
                return new MvcHtmlString(pathImagemPadrao);
            }

            string pathImagem = ONoticia.Foto.path;

            if (flagThumb) {
                pathImagem = Path.Combine(UtilString.notNull(ONoticia.Foto.pathThumb), "sistema", UtilString.notNull(ONoticia.Foto.nomeArquivo));
            }

            if (!File.Exists(Path.Combine(UtilConfig.pathAbsUploadFiles, pathImagem))) {
                return new MvcHtmlString(pathImagemPadrao);
            }

            return new MvcHtmlString(pathImagem);
        }

    }
}