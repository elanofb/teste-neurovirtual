using System.Data.Entity.ModelConfiguration;
using System;
using DAL.Organizacoes;
using DAL.Unidades;

namespace DAL.DocumentosFiscais {

    //
    public class UnidadeConfiguracao {

        public int id { get; set; }

	    public int idOrganizacao { get; set; }

	    public virtual Organizacao Organizacao { get; set; }
	    
	    public int idUnidade { get; set; }

	    public virtual Unidade Unidade { get; set; }

	    public bool? flagEmissaoNFSe { get; set; }
	    
	    public bool? flagProducaoNFSe { get; set; }
	    
	    public int? nroInicioNFSe { get; set; }
	    
	    public byte? serieInicioNFSe { get; set; }
	    
	    public bool? flagOptanteSimplesNacional { get; set; }
		
	    public bool? flagIncentivadorCultural { get; set; }
		
	    public byte? idExigibilidadeISS { get; set; }
	    
	    public bool? flagIncentivoFiscal { get; set; }
	    
	    public byte? idTipoTributacao { get; set; }
	    
	    public byte? idNaturezaTributacao { get; set; }
	    
	    public byte? idRegimeEspecialTributacao { get; set; }
	    
	    public int? idCertificadoEmissao { get; set; }

	    public int idUsuarioCadastro { get; set; }

        public DateTime dtCadastro { get; set; }

        public int? idUsuarioExclusao { get; set; }

        public DateTime? dtExclusao { get; set; }
    }

	//
	internal sealed class UnidadeConfiguracaoMapper : EntityTypeConfiguration<UnidadeConfiguracao> {

		public UnidadeConfiguracaoMapper() {
			this.ToTable("tb_unidade_configuracao");
			this.HasKey(x => x.id);

			this.HasRequired(x => x.Organizacao).WithMany().HasForeignKey(o => o.idOrganizacao);
			this.HasRequired(x => x.Unidade).WithMany().HasForeignKey(o => o.idUnidade);
		}
	}
}