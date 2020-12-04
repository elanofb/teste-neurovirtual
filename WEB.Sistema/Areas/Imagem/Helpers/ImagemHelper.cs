using System;
using System.Web;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Web.Helpers;
using System.IO;
 

namespace WEB.Areas.Imagem.Helpers {
    public static class ImagemHelper{

		//Constantes
		public static string pathImgPadrao = "~/Areas/Imagem/img/sem-imagem.jpg";


		/**
		 * Método para redimensionamento de imagens em tempo de execução.
		 */ 
		public static Bitmap redimensionar(Bitmap imgOriginal, int novaLargura, int novaAltura = 0){

			if(novaAltura == 0 && novaAltura == 0) return imgOriginal;

			if (novaAltura == 0) { 
				novaAltura = (int)Math.Round(imgOriginal.Height * (decimal)novaLargura / imgOriginal.Width, 0);
			}

			if (novaLargura == 0) { 
				novaLargura = (int)Math.Round(imgOriginal.Width * (decimal)novaAltura / imgOriginal.Height, 0);
			}

			var imagemDestino = new Bitmap(novaLargura, novaAltura);

			using (Graphics g = Graphics.FromImage(imagemDestino)){
				g.SmoothingMode = SmoothingMode.AntiAlias;
				g.InterpolationMode = InterpolationMode.HighQualityBicubic;
				g.PixelOffsetMode = PixelOffsetMode.HighQuality;
				g.DrawImage(imgOriginal, 0, 0, novaLargura, novaAltura);
			}

			return imagemDestino;
		}

		/**
		* Exibir ícones de imagens
		*/
		//public static void exibirIcone(string extensao, int largura = 0, int altura = 0) {
			
		//	string diretorioImagens64 = Server.MapPath("~/Areas/Imagem/img/icons/64/");
		//	string pathImagemDefault = Server.MapPath("~/Areas/Imagem/img/");

		//	string pathIcone = Path.Combine(UtilConfig.pathAbsRaiz, "img/sem-imagem.gif");

		//	if (extensao == ".mp3" || extensao == ".mp4") {
		//		pathIcone = Path.Combine(UtilConfig.pathAbsRaiz, "img/icons/64/audio.png");
		//	} else if (extensao == ".pdf") {
		//		pathIcone = Path.Combine(UtilConfig.pathAbsRaiz, "img/icons/64/pdf.png");
		//	} else if (extensao == ".xls" || extensao == ".xlsx") {
		//		pathIcone = Path.Combine(UtilConfig.pathAbsRaiz, "img/icons/64/excel.png");
		//	} else if (extensao == ".doc" || extensao == ".docx") {
		//		pathIcone = Path.Combine(UtilConfig.pathAbsRaiz, "img/icons/64/word.png");
		//	} else if (extensao == ".zip" || extensao == ".rar") {
		//		pathIcone = Path.Combine(UtilConfig.pathAbsRaiz, "img/icons/64/zip.png");
		//	} else if (extensao == ".txt" || extensao == ".sql") {
		//		pathIcone = Path.Combine(UtilConfig.pathAbsRaiz, "img/icons/64/text.png");
		//	}

		//	ImagemHelper.exibirImagem(pathIcone, largura, altura);
		//}

        /**
		 * Retornar uma imagem 
		 */
        public static void exibirImagem(string pathImagem, int novaLargura = 0, int novaAltura = 0) {
			string imgPadrao = HttpContext.Current.Server.MapPath("~/Areas/Imagem/img/sem-imagem.gif");
			pathImagem = HttpContext.Current.Server.UrlDecode(pathImagem);

			if (!File.Exists(pathImagem)) { 
				pathImagem = imgPadrao;
			}

			if(novaAltura == 0 && novaAltura == 0){
                new WebImage(pathImagem).Write();
				return;
			}

			var Imagem = Image.FromFile(pathImagem);
			if (novaAltura == 0) { 
				novaAltura = (int)Math.Round(Imagem.Height * (decimal)novaLargura / Imagem.Width, 0);
			}

			if (novaLargura == 0) { 
				novaLargura = (int)Math.Round(Imagem.Width * (decimal)novaAltura / Imagem.Height, 0);
			}

			new WebImage(pathImagem).Resize(novaLargura, novaAltura, true, false).Write();
        }


    }
}
