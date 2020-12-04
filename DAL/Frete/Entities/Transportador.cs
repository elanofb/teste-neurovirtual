using System.Data.Entity.ModelConfiguration;
using DAL.Entities;

namespace DAL.Frete {

	//
	public class Transportador : Geral {

		public string nome { get; set; }

		public string cnpj { get; set; }
	}

	//
	internal sealed class TransportadorMapper : EntityTypeConfiguration<Transportador> {

		public TransportadorMapper() {

            this.ToTable("datatb_transportador");

            this.HasKey(o => o.id);
		}
	}
}