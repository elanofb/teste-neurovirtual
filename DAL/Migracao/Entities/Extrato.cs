using System;
using System.Data;
using System.Data.Entity.ModelConfiguration;
using DAL.Associados;
using DAL.Entities;
using DAL.Organizacoes;

namespace DAL.Migracoes {

	public class Extrato {
		
	    public int id { get; set; }
		
		public int id_usuario { get; set; }
		
		public UsuarioGeral UsuarioGeral { get; set; }
		
		public int tipo { get; set; }
		
		public string mensagem { get; set; }
		
		public string valor { get; set; }
		
		public DateTime? data { get; set; }
		
		public DateTime? dtImportacao { get; set; }
		
	}
	
	internal sealed class ExtratoMapper : EntityTypeConfiguration<Extrato> {
		
		public ExtratoMapper() {
			
			this.ToTable("extrato");
			
			this.HasKey(x => x.id);			
			
			this.Ignore(x => x.dtImportacao);
			
			this.HasRequired(t => t.UsuarioGeral).WithMany().HasForeignKey(t => t.id_usuario);
			
		}
	}
}