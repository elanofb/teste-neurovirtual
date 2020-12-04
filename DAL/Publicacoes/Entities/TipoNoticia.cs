using System.Data.Entity.ModelConfiguration;
using DAL.Entities;

namespace DAL.Publicacoes {

	public partial class TipoNoticia : Geral {
	}


	internal sealed class TipoNoticiaMapper : EntityTypeConfiguration<TipoNoticia> {

		public TipoNoticiaMapper() {
			this.ToTable("datatb_tipo_noticia");
			this.HasKey(o => o.id);
		}
	}
}