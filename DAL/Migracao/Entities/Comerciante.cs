using System;
using System.Data;
using System.Data.Entity.ModelConfiguration;
using DAL.Associados;
using DAL.Entities;
using DAL.Organizacoes;

namespace DAL.Migracoes {

	public class Comerciante {
		
	    public int id { get; set; }
		
		public int? ind { get; set; }
		
		public int id_user { get; set; }
		
		public UsuarioComerciante UsuarioComerciante { get; set; }
		
		public string nome { get; set; }
		
		public string telefone { get; set; }
		
		public string funcionamento { get; set; }
		
		public string tipo { get; set; }
		
		public string rua { get; set; }
		
		public string numero { get; set; }
		
		public string bairro { get; set; }
		
		public string cidade { get; set; }
		
		public string estado { get; set; }
		
		public string pais { get; set; }
		
		public string cep { get; set; }
		
		public string desconto { get; set; }
		
		public string datahora { get; set; }
		
		public string ip { get; set; }
		
		public string status { get; set; }
						
		public string foto { get; set; }

	}
	
	internal sealed class ComercianteMapper : EntityTypeConfiguration<Comerciante> {
		
		public ComercianteMapper() {
			
			this.ToTable("comerciante");
			
			this.HasKey(x => x.id);						
			
			this.HasRequired(t => t.UsuarioComerciante).WithMany().HasForeignKey(t => t.id_user);
			
		}
	}
}