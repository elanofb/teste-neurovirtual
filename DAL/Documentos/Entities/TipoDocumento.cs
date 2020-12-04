using System;
using System.Data.Entity.ModelConfiguration;
using DAL.Entities;

namespace DAL.Documentos {

	[Serializable]
	public class TipoDocumento : Geral {

		public string nome { get; set; }

		public int idCategoriaDocumento { get; set; }

		public virtual CategoriaDocumento CategoriaDocumento { get; set; }

        public bool? flagPF { get; set; }

        public bool? flagPJ { get; set; }
	}

	//
	internal sealed class TipoDocumentoMapper : EntityTypeConfiguration<TipoDocumento> {

		public TipoDocumentoMapper() {

            this.ToTable("datatb_tipo_documento");

            this.HasKey(o => o.id);

			this.HasRequired(x => x.CategoriaDocumento).WithMany().HasForeignKey(x => x.idCategoriaDocumento);
		}
	}
}