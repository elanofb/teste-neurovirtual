using System;
using System.Data.Entity.ModelConfiguration;
using DAL.Organizacoes;
using DAL.Permissao;

namespace DAL.ConfiguracoesEcommerce {

	//
	public class ConfiguracaoEcommerce {

	    public int id { get; set; }

	    public int? idOrganizacao { get; set; }

	    public virtual Organizacao Organizacao {get; set;}

        public bool? flagHabilitarCompraAreaAssociado { get; set; }

		public bool? flagSomenteAssociados { get; set; }

		public bool? flagDirecionarAposIncluirProduto { get; set; }

		public bool? flagHabilitarFreteGratuito { get; set; }

		public decimal? valorParaFreteGratuito { get; set; }

        public bool? flagHabilitarCupomDesconto { get; set; }

        public string cepOrigemFrete { get; set; }

        public byte? qtdeLimiteParcelas { get; set; }

        public bool? flagCartaoCreditoPermitido { get; set; }

        public bool? flagBoletoBancarioPermitido { get; set; }

        public bool? flagDepositoPermitido { get; set; }

        public byte? qtdeDiasVencimento { get; set; }

        public int? idCentroCusto { get; set; }

        public int? idMacroConta { get; set; }

        public int? idCategoriaTitulo { get; set; }

        public int? idContaBancaria { get; set; }

       
	    public DateTime dtCadastro { get; set; }

	    public int? idUsuarioCadastro { get; set; }

	    public virtual UsuarioSistema UsuarioCadastro { get; set; }

	    public DateTime? dtExclusao { get; set; }

	    public int? idUsuarioExclusao { get; set; }

	}

	//
	internal sealed class ConfiguracaoEcommerceMapper : EntityTypeConfiguration<ConfiguracaoEcommerce> {

		public ConfiguracaoEcommerceMapper() {
			
		    this.ToTable("systb_configuracao_ecommerce");

		    this.HasKey(o => o.id);

		    this.HasOptional(x => x.UsuarioCadastro).WithMany().HasForeignKey(x => x.idUsuarioCadastro);

		    this.HasOptional(x => x.Organizacao).WithMany().HasForeignKey(x => x.idOrganizacao);
        }

	}

}