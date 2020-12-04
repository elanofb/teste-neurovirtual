using System;
using System.Data.Entity.ModelConfiguration;

namespace DAL.Localizacao {

	[Serializable]
	public class Pais {

		public string id { get; set; }

        public int? idPaisBACEN { get; set; }

        public int? idUsuarioCadastro { get; set; }

        public DateTime? dtCadastro { get; set; }

        public int? idUsuarioAlteracao { get; set; }

        public DateTime? dtAlteracao { get; set; }

		public string nome { get; set; }

		public string ativo { get; set; }

		public string flagExcluido { get; set; }

		public string flagSistema { get; set; }
	}

	internal sealed class PaisMapper : EntityTypeConfiguration<Pais> {

		public PaisMapper() {
			this.ToTable("datatb_pais");
			this.HasKey(o => o.id);
		}
	}
}