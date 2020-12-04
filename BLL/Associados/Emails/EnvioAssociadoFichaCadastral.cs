using System;
using System.Collections.Generic;
using DAL.Associados;
using DAL.Configuracoes;
using DAL.Pessoas;
using System.Linq;
using BLL.Configuracoes;
using BLL.Pessoas;
using DAL.Enderecos;

namespace BLL.Email {

	public class EnvioAssociadoFichaCadastral : EnvioEmailAdapter {

		//Atributos

		//Propriedades

		//Private Construtor
		private EnvioAssociadoFichaCadastral(int idOrganizacaoParam, List<string> listaDestino, List<string> listaCopia, List<string> listaCopiaOculta) :
			base(idOrganizacaoParam, listaDestino, listaCopia, listaCopiaOculta) {
		}

		//Factory
		public static EnvioAssociadoFichaCadastral factory(int idOrganizacaoParam, List<string> listaDestino, List<string> listaCopia, List<string> listaCopiaOculta = null) { 
			return new EnvioAssociadoFichaCadastral(idOrganizacaoParam, listaDestino, listaCopia, listaCopiaOculta);
		}

		//Customizado para essa classe especifica
		public UtilRetorno enviar(Associado OAssociado) { 

			Dictionary<string, object> infos = new Dictionary<string,object>();
            
            infos["nome"] = OAssociado.Pessoa.flagTipoPessoa == "F" ? "Nome: " + OAssociado.Pessoa.nome : "Razão Social: " + OAssociado.Pessoa.razaoSocial + "<br>Nome Fantasia: " + OAssociado.Pessoa.nome;

            infos["descDocumento"] = OAssociado.Pessoa.TipoDocumento != null ? OAssociado.Pessoa.TipoDocumento.nome : "Documento";

            infos["nroDocumento"] = UtilString.formatCPFCNPJ(OAssociado.Pessoa.nroDocumento);

            //Lista de e-mail
		    var email = OAssociado.Pessoa.emailPrincipal();
		    email = String.IsNullOrEmpty(OAssociado.Pessoa.emailSecundario()) ? email : String.Concat(email, " / ", OAssociado.Pessoa.emailSecundario());

            infos["email"] = email;

            //Lista de telefones
            var telefone = OAssociado.Pessoa.formatarTelPrincipal();
		    telefone = String.IsNullOrEmpty(OAssociado.Pessoa.formatarTelSecundario()) ? telefone : String.Concat(telefone, " / ", OAssociado.Pessoa.formatarTelSecundario());
		    telefone = String.IsNullOrEmpty(OAssociado.Pessoa.formatarTelTerciario()) ? telefone : String.Concat(telefone, " / ", OAssociado.Pessoa.formatarTelTerciario());

            infos["telefone"] = telefone;

            var nroAssociado = UtilString.notNull(OAssociado.nroAssociado);
			infos["id"] = !String.IsNullOrEmpty(nroAssociado) ? nroAssociado :  OAssociado.id.ToString();

			infos["descricaoTipoAssociado"] = (OAssociado.TipoAssociado == null? "-": OAssociado.TipoAssociado.descricao);

            var OEndereco = OAssociado.Pessoa.listaEnderecos.Count > 0 ? 
                            OAssociado.Pessoa.listaEnderecos.FirstOrDefault() : new PessoaEndereco();

            infos["endereco"] = OEndereco.id > 0 ? 
                                String.Concat(
                                    OEndereco.logradouro, " ", OEndereco.numero, " ", OEndereco.complemento, " - ", OEndereco.bairro, " - ", 
                                    (OEndereco.Cidade != null ? OEndereco.Cidade.nome : OEndereco.nomeCidade), "/", 
                                    (OEndereco.Cidade != null && OEndereco.Cidade.Estado != null ? OEndereco.Cidade.Estado.sigla : OEndereco.uf)
                                ) : "-";

            infos["link"] = String.Concat(UtilConfig.linkAbsSistema, "Associados/associadoimpressao/visualizar-admissao/", UtilCrypt.toBase64Encode(OAssociado.id));

		    ConfiguracaoNotificacao OConfiguracao = ConfiguracaoNotificacaoBL.getInstance.carregar(this.idOrganizacao);

		    var assuntoEmail = OConfiguracao.assuntoEmailFichaAssociado.Replace("#NOME#", OAssociado.Pessoa.nome);

			return this.enviar(infos, assuntoEmail);
		}

		//Sobreposicao para designar corpo do e-mail
		//Como vamos capturar o conteudo do banco de dados, nao precisamos informar o arquivo html do email no parametro
		protected override string capturarConteudoHTML(string arquivoHTML) {

			ConfiguracaoNotificacao OConfiguracao = ConfiguracaoNotificacaoBL.getInstance.carregar(this.idOrganizacao);

			string conteudoHTML = OConfiguracao.corpoEmailFichaAssociado;

			string htmlMaster = this.capturarMasterpage("");

			string htmlFinal = htmlMaster.Replace("#CONTEUDO_MENSAGEM#", conteudoHTML);
			
			htmlFinal = htmlFinal.Replace("#NOME_APLICACAO#", OConfiguracaoSistema.tituloSistema);
			
			return htmlFinal;
		}

		//Sobreposicao obrigatorio do metodo abstrato
		public override UtilRetorno enviar(IDictionary<string, object> info, string assunto) {
				
			this.Subject = assunto;

			this.prepararMensagem("");

			this.Body = this.Body.Replace("#NOME#", info["nome"].ToString());

			this.Body = this.Body.Replace("#DESCRICAO_DOCUMENTO#", info["descDocumento"].ToString());

            this.Body = this.Body.Replace("#NRO_DOCUMENTO#", info["nroDocumento"].ToString());

            this.Body = this.Body.Replace("#EMAIL#", info["email"].ToString());

            this.Body = this.Body.Replace("#TELEFONE#", info["telefone"].ToString());

			this.Body = this.Body.Replace("#NUMERO_ASSOCIADO#", info["id"].ToString());

			this.Body = this.Body.Replace("#TIPO_ASSOCIADO#", info["descricaoTipoAssociado"].ToString());

            this.Body = this.Body.Replace("#ENDERECO#", info["endereco"].ToString());

            this.Body = this.Body.Replace("#LINK#", info["link"].ToString());

            this.Body = this.Body.Replace("#DATA_EMAIL#", DateTime.Now.ToLongDateString());

			return this.disparar();
		}
	}
}