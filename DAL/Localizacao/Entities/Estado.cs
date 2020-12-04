using System;
using System.Data.Entity.ModelConfiguration;

namespace DAL.Localizacao {
	
    //
	[Serializable]
	public class Estado : Localidade {

        public int? idIBGE { get; set; }

		public string sigla { get; set; }

        public string idPais { get; set; }

        public virtual Pais Pais { get; set; }

	}

	//
	internal sealed class EstadoMapper : EntityTypeConfiguration<Estado> {

		public EstadoMapper() {
			this.ToTable("datatb_estado");
			this.HasKey(o => o.id);

            this.HasOptional(x => x.Pais).WithMany().HasForeignKey(x => x.idPais);
		}

	}
}