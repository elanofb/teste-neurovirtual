using System.Data.Entity.ModelConfiguration;
using System;
using DAL.Unidades;

namespace DAL.DocumentosFiscais {

    //
    public class UnidadeNumeracaoDocumento {

        public int id { get; set; }

	    public int idOrganizacao { get; set; }

	    public int idUnidade { get; set; }

        public virtual Unidade Unidade { get; set; }
        
        public bool? flagNFSe { get; set; }

        public bool? flagNFe { get; set; }

        public int? nroDocumento { get; set; }
	    
        public byte? serie { get; set; }

        public DateTime? dtUtilizacao { get; set; }

        public DateTime dtCadastro { get; set; }

        public int idUsuarioCadastro { get; set; }

        public DateTime? dtExclusao { get; set; }

        public int? idUsuarioExclusao { get; set; }
        
    }

	//
	internal sealed class UnidadeNumeracaoDocumentoMapper : EntityTypeConfiguration<UnidadeNumeracaoDocumento> {

		public UnidadeNumeracaoDocumentoMapper() {

			this.ToTable("tb_unidade_numeracao_documento");

			this.HasKey(x => x.id);

			this.HasRequired(x => x.Unidade).WithMany().HasForeignKey(o => o.idUnidade);
            
		}

	}

}