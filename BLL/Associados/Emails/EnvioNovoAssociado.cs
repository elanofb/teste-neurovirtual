using System;
using System.Collections.Generic;
using BLL.Pessoas;
using DAL.Associados;
using DAL.Configuracoes;
using BLL.Configuracoes;
using DAL.Configuracoes.Default;

namespace BLL.Email {

	public class EnvioNovoAssociado : EnvioEmailAdapter {

		//Atributos

		//Propriedades

		//Private Construtor
		private EnvioNovoAssociado(int idOrganizacaoParam, List<string> listaDestino, List<string> listaCopia, List<string> listaCopiaOculta) :
			base(idOrganizacaoParam, listaDestino, listaCopia, listaCopiaOculta) {
		}

		//Factory
		public static EnvioNovoAssociado factory(int idOrganizacaoParam, List<string> listaDestino, List<string> listaCopia, List<string> listaCopiaOculta = null) { 
			return new EnvioNovoAssociado(idOrganizacaoParam, listaDestino, listaCopia, listaCopiaOculta);
		}

		//Customizado para essa classe especifica
		public UtilRetorno enviar(Associado OAssociado) {

		    idOrganizacao = OAssociado.idOrganizacao;

			Dictionary<string, object> infos = new Dictionary<string,object>();

            var nroAssociado = UtilString.notNull(OAssociado.nroAssociado);

			infos["id"] = !String.IsNullOrEmpty(nroAssociado) ? nroAssociado :  OAssociado.id.ToString();

			infos["descricaoTipoAssociado"] = (OAssociado.TipoAssociado == null? "-": OAssociado.TipoAssociado.descricao);

			infos["nome"] = OAssociado.Pessoa.nome;

		    OConfiguracaoSistema = ConfiguracaoSistemaBL.getInstance.carregar(OAssociado.idOrganizacao);

			return this.enviar(infos, $"Seja bem vindo à {OConfiguracaoSistema.tituloSistema}");
		}

		//Sobreposicao para designar corpo do e-mail
		//Como vamos capturar o conteudo do banco de dados, nao precisamos informar o arquivo html do email no parametro
		protected override string capturarConteudoHTML(string arquivoHTML) {

			ConfiguracaoNotificacao OConfiguracao = ConfiguracaoNotificacaoBL.getInstance.carregar(idOrganizacao);

			string conteudoHTML = OConfiguracao.corpoEmailNovoAssociado;

			string htmlMaster = this.capturarMasterpage("");
            
			string htmlFinal = htmlMaster.Replace("#CONTEUDO_MENSAGEM#", conteudoHTML);
			
			htmlFinal = htmlFinal.Replace("#NOME_APLICACAO#", OConfiguracaoSistema.tituloSistema);
			
			return htmlFinal;
		}

		//Sobreposicao obrigatorio do metodo abstrato
		public override UtilRetorno enviar(IDictionary<string, object> info, string assunto) {

			this.Subject = assunto;

			this.prepararMensagem("");

			this.Body = this.Body.Replace("#NUMERO_ASSOCIADO#", info["id"].ToString());

			this.Body = this.Body.Replace("#TIPO_ASSOCIADO#", info["descricaoTipoAssociado"].ToString());

			this.Body = this.Body.Replace("#NOME#", info["nome"].ToString());

            this.Body = this.Body.Replace("#DATA_EMAIL#", DateTime.Now.ToLongDateString());

            this.Body = this.Body.Replace("#COMPLEMENTO_MENSAGEM_SUCESSO#", "");

            this.Body = this.Body.Replace("#NOME_ORGANIZACAO#", OConfiguracaoSistema.nomeEmpresaCompleto);

			return this.disparar();
		}
	}
}