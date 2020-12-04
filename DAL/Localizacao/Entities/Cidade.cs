using System;
using System.Data.Entity.ModelConfiguration;

namespace DAL.Localizacao {
	/**
	*
	*/
	
	[Serializable]
	public class Cidade : Localidade {
		
        public int? idMunicipioIBGE { get; set; }
					
		public string nomeMunicipio { get; set; }
		
		public int idEstado { get; set; }
		
		public virtual Estado Estado { get; set; }
		
        public string flagCapital { get; set; }
		
	}
	
	//
	internal sealed class CidadeMapper : EntityTypeConfiguration<Cidade> {
		
		public CidadeMapper() {
			
			this.ToTable("datatb_cidade");
			this.HasKey(o => o.id);
			
			//FKs
			this.HasRequired(u => u.Estado).WithMany().HasForeignKey(u => u.idEstado);
		}
	}
}