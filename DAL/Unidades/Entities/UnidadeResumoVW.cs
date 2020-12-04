using System.Data.Entity.ModelConfiguration;

namespace DAL.Unidades {

	//
	public class UnidadeResumoVW  {

        public int id { get; set; }

        public string sigla { get; set; }

        public string tipoUnidade { get; set; }

        public int? idUnidadeMatriz { get; set; }

		public string nomeFantasiaUnidade { get; set; }

		public string razaoSocialUnidade { get; set; }
		
		public string cnpjUnidade { get; set; }

        public int? idOrganizacao { get; set; }

        public bool? flagEmissaoNFSe { get; set; }

        public int? idCidade { get; set; }

		public string nomeCidade { get; set; }

		public string ativo { get; set; }

        public UnidadeResumoVW() {
		}
	}

	//
	internal sealed class UnidadeResumoVWMapper : EntityTypeConfiguration<UnidadeResumoVW> {

		public UnidadeResumoVWMapper() {

			this.ToTable("vw_unidade_resumo");

		    this.HasKey(o => o.id);

		}
	}
}