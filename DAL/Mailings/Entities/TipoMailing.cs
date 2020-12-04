using System.Data.Entity.ModelConfiguration;
using DAL.Entities;

namespace DAL.Mailings {

	public partial class TipoMailing : Geral {

        public string nome { get; set; }

        public string flagSistema { get; set; }

        public byte? qtdeLimiteEmail { get; set; }
    }

	internal sealed class TipoMailingMapper : EntityTypeConfiguration<TipoMailing> {

		public TipoMailingMapper() {

			this.ToTable("tb_tipo_mailing");

			this.HasKey(o => o.id);

		}
	}
}