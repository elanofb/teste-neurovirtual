using System.Data.Entity.ModelConfiguration;
using DAL.Entities;
using System;

namespace DAL.Contatos {

    public class TipoContatoVW {
	    
        public int id { get; set; }

	    public int? idOrganizacao { get; set; }

	    public string descricao { get; set; }

	    public bool? ativo { get; set; }
	    
        public DateTime dtCadastro { get; set; }

	    public int idUsuarioCadastro { get; set; }
	    
	    public string nomeUsuarioCadastro { get; set; }
	    
	    public bool flagSistema { get; set; }
	    
    }


	internal sealed class TipoContatoVWMapper : EntityTypeConfiguration<TipoContatoVW> {

		public TipoContatoVWMapper() {
			
			this.ToTable("vw_tipo_contato");
			
			this.HasKey(o => o.id);
			
		}
	}
}