using System;
using System.Web.Mvc;
using System.IO;
using DAL.Diretorias;

namespace WEB.Areas.Diretorias.Extensions {

    public static class DiretoriaMembroExtensions{
		
		//
        public static MvcHtmlString pathImagem(this DiretoriaMembro ODiretoriaMembro, bool flagThumb = false){
			string pathImagemPadrao = "default/sem-imagem.gif";

			if (ODiretoriaMembro.Arquivo == null) {
				return new MvcHtmlString(pathImagemPadrao);
			}

            string pathImagem = ODiretoriaMembro.Arquivo.path;

            if (flagThumb) {
                pathImagem = Path.Combine(UtilString.notNull(ODiretoriaMembro.Arquivo.pathThumb), "sistema", UtilString.notNull(ODiretoriaMembro.Arquivo.nomeArquivo));
            }

			if (!File.Exists(Path.Combine(UtilConfig.pathAbsUploadFiles, pathImagem))) {
				return new MvcHtmlString(pathImagemPadrao);
			}

			return new MvcHtmlString(pathImagem);
        }

    }
}