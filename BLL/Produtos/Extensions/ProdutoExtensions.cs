using System;
using System.Linq;
using DAL.Arquivos;
using DAL.Produtos;
using System.IO;

namespace BLL.Produtos {

    public static class ProdutoExtensions{
		
		//
        public static string pathImagem(this Produto OProduto, string localThumb = ""){

			string pathImagemPadrao = "default/sem-imagem.gif";

			if (OProduto.listaFotos.Count == 0) {

				return pathImagemPadrao;

			}

			ArquivoUpload OFoto = OProduto.listaFotos.FirstOrDefault();

            string pathImagem = OFoto.path;

            if (!string.IsNullOrEmpty(localThumb)) {

                pathImagem = Path.Combine(UtilString.notNull(OFoto.pathThumb), localThumb, UtilString.notNull(OFoto.nomeArquivo));

            }

			if (!File.Exists(Path.Combine(UtilConfig.pathAbsUploadFiles, pathImagem))) {

				return pathImagemPadrao;
			}

			return (pathImagem);
        }

		//Link completo da imagem
		public static string linkImagem(this Produto OProduto, string localThumb = "") {

			return string.Concat(UtilConfig.linkAbsSistema, "upload/", OProduto.pathImagem(localThumb) );
		}
	}
}
