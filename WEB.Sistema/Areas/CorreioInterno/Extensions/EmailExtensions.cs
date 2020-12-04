using DAL.Entities;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace System {

	public static class EmailExtensions {

		/**
		 * 
		 */
		public static string exibirRemetente(this LogEmail Email) {
			string remetente = "";

			if (!String.IsNullOrEmpty(Email.nomeRemetente)) { 
				remetente = String.Concat(Email.nomeRemetente, " <", Email.emailRemetente,"> ");
			} else { 
				 remetente = Email.emailRemetente;
			}

			return remetente;
		}

		/**
		 * 
		 */
		public static string exibirDestinos(this LogEmail Email) {
			
			var listaDestinos = Email.listaEmailDestino.Where(x => x.flagCopia == "N" && x.flagCopiaOculta == "N").ToList();
			string html = htmlContato(listaDestinos);

			return html;
		}

		/**
		 * 
		 */
		public static string exibirCopias(this LogEmail Email) {
			
			var listaDestinos = Email.listaEmailDestino.Where(x => x.flagCopia == "S" && x.flagCopiaOculta == "N").ToList();
			string html = htmlContato(listaDestinos);

			return html;
		}

		/**
		 * 
		 */
		public static string exibirCopiasOcultas(this LogEmail Email) {
			
			var listaDestinos = Email.listaEmailDestino.Where(x => x.flagCopia == "N" && x.flagCopiaOculta == "S").ToList();
			string html = htmlContato(listaDestinos);

			return html;
		}

		/**
		*
		*/
		private static string htmlContato(List<LogEmailDestino> listaDestinos) {
			System.Text.StringBuilder html = new Text.StringBuilder();

			//listaDestinos = listaDestinos.DistinctBy(x => x.emailDestino).ToList();
            listaDestinos = listaDestinos.ToList();

			listaDestinos.ForEach(item => {
				if (!String.IsNullOrEmpty(item.nomeDestino) && !item.nomeDestino.Equals(item.emailDestino)) { 
					html.Append(String.Concat(item.nomeDestino, " <", item.emailDestino,"> "));
				} else { 
					 html.Append(item.emailDestino);
				}
				html.Append("; ");
			});

			return html.ToString();
		}

		/**
		 * 
		 */
		public static string exibirDestinosEnviados(this LogEmail Email) {

			var listaDestinos = Email.listaEmailDestino.Where(x => x.flagCopia == "N" && x.flagCopiaOculta == "N").ToList();
			StringBuilder html = new StringBuilder();

			listaDestinos.ForEach(item => {
				if(!String.IsNullOrEmpty(item.nomeDestino)){
					html.Append(item.nomeDestino);
				}else{
					html.Append(item.emailDestino);
				}
			});

			return html.ToString();
		}

		/**
		 * 
		 */
		public static string exibirMensagem(this LogEmail Email, bool flagConfiavel = false) {
			string mensagem = Email.corpoMensagem.Replace("script", "#script#");
			
			if (!flagConfiavel) {
				mensagem = mensagem.Replace("http", "#http#");
			}

			mensagem = mensagem.Replace("\n", "<br />");

			return mensagem;
		}


		/**
		 * 
		 */
		public static string cssClasseLeitura(this LogEmail Email) {
			return Email.dtPrimeiraAbertura.HasValue? "lido": "nao-lido";
		}
	}
}