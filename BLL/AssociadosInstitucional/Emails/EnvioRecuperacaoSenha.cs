using System;
using System.Collections;
using System.Collections.Generic;
using DAL.Associados;
using BLL.Email;
using BLL.Pessoas;

namespace BLL.AssociadosInstitucional.Emails {

	public class EnvioRecuperacaoSenha : EnvioEmailAdapter {


		//Private Construtor
		private EnvioRecuperacaoSenha(int idOrganizacaoParam, List<string> listaDestino, List<string> listaCopia, List<string> listaCopiaOculta) :
			base(idOrganizacaoParam, listaDestino, listaCopia, listaCopiaOculta) {
		}

		//Factory
		public static EnvioRecuperacaoSenha factory(int idOrganizacaoParam, List<string> listaDestino, List<string> listaCopia) { 
			return new EnvioRecuperacaoSenha(idOrganizacaoParam, listaDestino, listaCopia, null);
		}

		//Método próprio para recuperacao de senha
		public UtilRetorno enviar(Associado OAssociado, string novaSenha) {

			IDictionary<string, object> info = new Dictionary<string, object>();
			info["nomeAssociado"] = OAssociado.Pessoa.nome;
			info["login"] = OAssociado.Pessoa.login;
			info["novaSenha"] = novaSenha;

			return this.enviar(info);
		} 

		//Sobreposicao obrigatorio do metodo abstrato
		public override UtilRetorno enviar(IDictionary<string, object> info, string assunto = "Recuperação de Senha") {

			this.Subject = assunto;
			
			this.prepararMensagem("envio-recuperacao-senha.html");
			
			this.Body = this.Body.Replace("#NOME_ASSOCIADO#", info["nomeAssociado"].ToString());

			this.Body = this.Body.Replace("#NOVASENHA#", info["novaSenha"].ToString());

			this.Body = this.Body.Replace("#LOGIN#", info["login"].ToString());

			return this.disparar();
		}
	}
}