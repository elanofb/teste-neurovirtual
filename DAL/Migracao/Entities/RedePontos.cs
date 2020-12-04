using System;
using System.Data.Entity.ModelConfiguration;

namespace DAL.Migracoes {

	public class RedePontos {
		
	    public int id { get; set; }
		
		public int id_usuario { get; set; }
		
		public int? pontos { get; set; }
		
		public int chave_binaria { get; set; }
		
		public int pago { get; set; }
		
		public DateTime? data { get; set; }
		
	}
	
	internal sealed class RedePontosMapper : EntityTypeConfiguration<RedePontos> {
		
		public RedePontosMapper() {
			
			this.ToTable("rede_pontos_binario");
			
			this.HasKey(x => x.id);			
			
			
		}
	}
}