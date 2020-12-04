using System;
using System.Web;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Web.Helpers;

namespace WEB.Areas.Arquivos.Helpers {
    public static class ImagemHelper{

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
		 * Retornar uma imagem 
		 */
        public static void exibirImagem(string pathImagem, int novaLargura = 0, int novaAltura = 0) {
			pathImagem = HttpContext.Current.Server.UrlDecode(pathImagem);
			
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
