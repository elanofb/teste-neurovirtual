using System;
using System.Data;
using System.Data.Entity.ModelConfiguration;
using DAL.Associados;
using DAL.Entities;
using DAL.Organizacoes;

namespace DAL.Migracoes {

	public class Rede {
		
	    public int id { get; set; }
		
		public int id_usuario { get; set; }
		
		public int id_patrocinador { get; set; }
		
		public int id_patrocinador_direto { get; set; }
		
		public int chave_binaria { get; set; }
		
		public int plano_ativo { get; set; }
		
		public bool? flagImportado { get; set; }
		
	}
	
	internal sealed class RedeMapper : EntityTypeConfiguration<Rede> {
		
		public RedeMapper() {
			
			this.ToTable("rede");
			
			this.HasKey(x => x.id);			
			
			this.Ignore(x => x.flagImportado);
			
		}
	}
}