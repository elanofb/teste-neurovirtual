using System.Data.Entity.ModelConfiguration;

namespace DAL.Unidades {

	//
	public class UnidadeEmissoraVW  {

        public int id { get; set; }

        public int idPessoa { get; set; }

        public string sigla { get; set; }

        public string cnpj { get; set; }

        public string ie { get; set; }

		public string nomeFantasia { get; set; }

        public string razaoSocial { get; set; }

		public string logradouro { get; set; }

		public string numero { get; set; }

		public string complemento { get; set; }

		public string bairro { get; set; }

		public string cep { get; set; }

        public int? idCidade { get; set; }

        public int? idMunicipioIBGE { get; set; }

		public string nomeCidade { get; set; }

		public string nomeEstado { get; set; }

		public string siglaEstado { get; set; }

        public int? idEstadoIBGE { get; set; }

		public bool? flagCTEProducao { get; set; }
        
		public bool? flagEmissaoAutomatica { get; set; }

		public string nroRNTRC { get; set; }

		public string nomeCertificado { get; set; }

        public int? idCertificadoEmissaoCTe { get; set; }

        public string telPrincipal { get; set; }

        public string telSecundario { get; set; }

        public string email { get; set; }


        public UnidadeEmissoraVW() {
		}
	}

	//
	internal sealed class UnidadeEmissoraVWMapper : EntityTypeConfiguration<UnidadeEmissoraVW> {

		public UnidadeEmissoraVWMapper() {

			this.ToTable("vw_unidade_emissora");

		    this.HasKey(o => o.id);

		}
	}
}