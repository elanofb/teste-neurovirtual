using System;
using System.Web.Mvc;
using System.IO;
using WEB.Areas.Imagem.Helpers;

namespace WEB.Areas.Imagem.Controllers {

	public class DefaultController : Controller {

		//Constantes

		//Atributos

		//Propriedades

		//Eventos

		/**
		* Exibição de imagens em tempo de execucao
		* PathImagem: Caminho do arquivo a partir do diretório raíz de imagens
		* largura: Tamanho que deve ser exibido em tempo de execução (se nao informar, usará a proporção)
		* altura: Altura que deve ser exibida em tempo de execução (se nao informar, usará a proporção)
		* pathAlternativa: caso as pasta de imagens seja de um caminho diferente do padrão
		*/
		[ActionName("exibir-imagem"), AllowAnonymous]
		public void exibirImagem(string pathImagem, int largura = 0, int altura = 0) {

			pathImagem = Path.Combine(UtilConfig.pathAbsUploadFiles, pathImagem);

			ImagemHelper.exibirImagem(pathImagem, largura, altura);
		}



	}
}