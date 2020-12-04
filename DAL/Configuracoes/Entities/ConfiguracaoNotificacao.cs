using System;
using System.Data.Entity.ModelConfiguration;
using DAL.Organizacoes;
using DAL.Permissao;

namespace DAL.Configuracoes {

	//
	public class ConfiguracaoNotificacao  {

		public int id { get; set; }

		public int? idOrganizacao { get; set; }

        public virtual Organizacao Organizacao { get; set; }

        public string novoUsuarioTitulo { get; set; }

		public string novoUsuarioCorpo { get; set; }

        public string reenvioSenhaAssociadoTitulo { get; set; }

        public string reenvioSenhaAssociadoCorpo { get; set; }

		public string tituloEmailRecuperacaoSenhaAssociado { get; set; }

		public string corpoEmailRecuperacaoSenhaAssociado { get; set; }

		public string tituloEmailRecuperacaoSenhaTransacaoAssociado { get; set; }

		public string corpoEmailRecuperacaoSenhaTransacaoAssociado { get; set; }

        public string reenvioSenhaUsuarioTitulo { get; set; }

		public string reenvioSenhaUsuarioCorpo { get; set; }

		public string recuperacaoSenhaUsuarioTitulo { get; set; }

		public string recuperacaoSenhaUsuarioCorpo { get; set; }

		public string emailPedidos { get; set; }

		public string emailNovoAssociado { get; set; }

		public string emailNovoNaoAssociado { get; set; }

		public string emailContato { get; set; }

		public string emailInscricaoEvento { get; set; }

		public string emailCobrancaContribuicao { get; set; }

        public string emailAssociadoDegustacao { get; set; }

        public string corpoEmailNovoAssociado { get; set; }

		public string corpoEmailNovoNaoAssociado { get; set; }

        public string assuntoEmailFichaAssociado { get; set; }

        public string corpoEmailFichaAssociado { get; set; }

        public string corpoEmailAssociadoDegustacao { get; set; }

        public string assuntoEmailFichaNaoAssociado { get; set; }

        public string corpoEmailFichaNaoAssociado { get; set; }
        
		public string corpoEmailCobrancaAnuidade { get; set; }

		public string corpoEmailPagamentoAnuidade { get; set; }

		public string tituloEmailCobrancaContribuicao { get; set; }

		public string corpoEmailCobrancaContribuicao { get; set; }

		public string tituloEmailPagamentoContribuicao { get; set; }

        public string corpoEmailPagamentoContribuicao { get; set; }

		public string corpoEmailNovaInscricaoEvento { get; set; }

		public string corpoEmailPagamentoInscricao { get; set; }

        public string corpoEmailIsencaoInscricao { get; set; }

		public string corpoEmailCobrancaInscricaoEvento { get; set; }        

        public string corpoEmailEnvioCerficadoEvento { get; set; }                

        public string tituloEmailNovoPedido { get; set; }

		public string corpoEmailNovoPedido { get; set; }

        public string tituloEmailFaturamentoPedido { get; set; }

        public string corpoEmailFaturamentoPedido { get; set; }

        public string tituloEmailCobrancaPedido { get; set; }

		public string corpoEmailCobrancaPedido { get; set; }

        public string tituloEmailPagamentoPedido { get; set; }

		public string corpoEmailPagamentoPedido { get; set; }

		public string corpoEmailNovoPlano { get; set; }

		public string corpoEmailPagamentoPlano { get; set; }

	    public string tituloEmailCobranca { get; set; }

        public string corpoEmailCobranca { get; set; }

        public string corpoEmailNovaNotificacao { get; set; }

        public string assuntoEmailRecusaPagamento { get; set; }

        public string corpoEmailRecusaPagamento { get; set; }

        public string tituloEmailPagamentoRecebido { get; set; }

        public string corpoEmailPagamentoRecebido { get; set; }

	    public string assuntoEmailMensagemAtendimento { get; set; }

        public string corpoEmailMensagemAtendimento { get; set; }

        public DateTime dtCadastro { get; set;}

        public int idUsuarioCadastro { get; set; }

        public virtual UsuarioSistema UsuarioCadastro { get; set; }

        public bool? flagExcluido { get; set; }
	}

	//
	internal sealed class ConfiguracaoNotificacaoMapper : EntityTypeConfiguration<ConfiguracaoNotificacao> {

		public ConfiguracaoNotificacaoMapper() {
			this.ToTable("systb_configuracao_notificacao");
			this.HasKey(o => o.id);

            this.HasOptional(x => x.Organizacao).WithMany().HasForeignKey(x => x.idOrganizacao);
            this.HasRequired(x => x.UsuarioCadastro).WithMany().HasForeignKey(x => x.idUsuarioCadastro);
        }
	}
}