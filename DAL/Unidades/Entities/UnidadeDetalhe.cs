namespace DAL.Unidades {

	//
	public class UnidadeDetalhe {

		public int idUnidade { get; set; }

		public string cnpj { get; set; }

		public string inscricaoEstadual { get; set; }

		public string inscricaoMunicipal { get; set; }

		public string razaoSocial { get; set; }

		public string nomeFantasia { get; set; }

		public string telComercial { get; set; }

		public string email { get; set; }

		public string logradouro { get; set; }

		public string numero { get; set; }

		public string complemento { get; set; }

		public string bairro { get; set; }

		public string cep { get; set; }

		public int? idCidade { get; set; }

		public string nomeCidade { get; set; }

		public int? idMunicipioIBGE { get; set; }

		public int? idEstado { get; set; }

		public string siglaEstado { get; set; }

		public string idPaisBACEN { get; set; }

		public string nomePais { get; set; }

		public UnidadeDetalhe() {
		}
	}


}