using System;
using System.Data.Entity.ModelConfiguration;

namespace DAL.Institucionais {

	[Serializable]
	public class ResultadoBuscaVW {

		public int id { get; set; }

        public int? idOrganizacao { get; set; }

        public int? idPortal { get; set; }

        public string titulo { get; set; }

		public string descricao { get; set; }

		public string informacaoData { get; set; }

		public string tipoResultado { get; set; }
	}

	//
	internal sealed class ResultadoBuscaVWMapper : EntityTypeConfiguration<ResultadoBuscaVW> {

		public ResultadoBuscaVWMapper() {
			this.ToTable("vw_busca_institucional");
			this.HasKey(o => o.id);
		}
	}
}