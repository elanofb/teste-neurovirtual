using System;
using System.Data.Entity.ModelConfiguration;

namespace DAL.Localizacao {

	public class CepBrasil {

		public virtual Int32 id { get; set; }

		public int? idEstado { get; set; }

		public string siglaEstado { get; set; }

		public int? idCidade { get; set; }

		public Cidade Cidade { get; set; }

		public string nomeCidade { get; set; }

		public string bairroIni { get; set; }

		public string bairroFim { get; set; }

		public string tipoLogradouro { get; set; }

		public string logradouro { get; set; }

		public string logradouroComplemento { get; set; }

		public string cepOriginal { get; set; }

		public string cepIni { get; set; }

		public string cepFim { get; set; }
	}

	/**
	*
	*/

	internal sealed class CepBrasilMapper : EntityTypeConfiguration<CepBrasil> {

		public CepBrasilMapper() {
			this.ToTable("systb_cep_brasil");
			this.HasKey(o => o.id);

			this.HasOptional(x => x.Cidade).WithMany().HasForeignKey(x => x.idCidade);
		}
	}
}