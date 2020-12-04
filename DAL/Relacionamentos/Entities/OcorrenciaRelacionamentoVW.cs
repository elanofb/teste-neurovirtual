using System.Data.Entity.ModelConfiguration;
using System;

namespace DAL.Relacionamentos {

    public class OcorrenciaRelacionamentoVW {
	    
        public int id { get; set; }

	    public int? idOrganizacao { get; set; }

	    public string descricao { get; set; }

	    public bool? ativo { get; set; }
	    
        public DateTime dtCadastro { get; set; }

	    public int idUsuarioCadastro { get; set; }
	    
	    public string nomeUsuarioCadastro { get; set; }
	    
	    public bool flagSistema { get; set; }
	    
    }


	internal sealed class OcorrenciaRelacionamentoVWMapper : EntityTypeConfiguration<OcorrenciaRelacionamentoVW> {

		public OcorrenciaRelacionamentoVWMapper() {
			
			this.ToTable("vw_ocorrencia_relacionamento");
			
			this.HasKey(o => o.id);
			
		}
	}
}