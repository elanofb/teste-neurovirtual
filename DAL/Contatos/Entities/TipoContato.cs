using System.Data.Entity.ModelConfiguration;
using DAL.Entities;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Contatos {

    public class TipoContato {
	    
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


	internal sealed class TipoContatoMapper : EntityTypeConfiguration<TipoContato> {

		public TipoContatoMapper() {
			
			this.ToTable("tb_tipo_contato");
			
			this.HasKey(o => o.id);
			
			this.Property(o => o.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
			
		}
	}
}