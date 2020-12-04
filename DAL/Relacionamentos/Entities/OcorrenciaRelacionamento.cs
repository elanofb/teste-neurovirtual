using System.Data.Entity.ModelConfiguration;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Relacionamentos {

    public class OcorrenciaRelacionamento {
	    
        public int id { get; set; }

	    public int? idOrganizacao { get; set; }

	    public string descricao { get; set; }

	    public bool? ativo { get; set; }
	    
        public DateTime dtCadastro { get; set; }

	    public int idUsuarioCadastro { get; set; }
	    
        public DateTime? dtAlteracao { get; set; }

        public int? idUsuarioAlteracao { get; set; }

        public DateTime? dtExclusao { get; set; }
	    
	    public int? idUsuarioExclusao { get; set; }
	    
    }


	internal sealed class OcorrenciaRelacionamentoMapper : EntityTypeConfiguration<OcorrenciaRelacionamento> {

		public OcorrenciaRelacionamentoMapper() {
			
			this.ToTable("tb_ocorrencia_relacionamento");
			
			this.HasKey(o => o.id);
			
			this.Property(o => o.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
			
		}
	}
}