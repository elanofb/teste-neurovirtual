using System;
using System.Collections.Generic;

namespace BLL.Email {

	public class EnvioCorreioInterno : EnvioEmailAdapter {


		//Private Construtor
		private EnvioCorreioInterno(int idOrganizacaoParam, List<string> listaDestino, List<string> listaCopia, List<string> listaCopiaOculta) :
			base(idOrganizacaoParam, listaDestino, listaCopia, listaCopiaOculta) {
		}

		//Factory
		public static EnvioEmailAdapter factory(int idOrganizacaoParam, List<string> listaDestino, List<string> listaCopia, List<string> listaCopiaOculta) { 
			return new EnvioCorreioInterno(idOrganizacaoParam, listaDestino, listaCopia, listaCopiaOculta);
		}

		//
		public override UtilRetorno enviar(IDictionary<string, object> infos, string assunto = "Nova Mensagem") {
			
			this.Subject = assunto;
			
			this.prepararMensagem("correio-interno-mensagem.html");
			
			this.Body = this.Body.Replace("#MENSAGEM#", infos["mensagem"].ToString());

			return this.disparar();
		}
	}
}