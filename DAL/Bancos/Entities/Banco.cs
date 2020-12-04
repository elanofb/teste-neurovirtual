using System.Data.Entity.ModelConfiguration;
using DAL.Entities;

namespace DAL.Bancos {
	
	public class Banco : Geral {

		public string nroBanco { get; set; }

		public string nome { get; set; }

        public string site { get; set; }

        public string flagSistema { get; set; }

	}

	
	internal sealed class BancoMapper : EntityTypeConfiguration<Banco> {

		public BancoMapper() {
			this.ToTable("datatb_banco");
			this.HasKey(x => x.id);
		}
	}
}