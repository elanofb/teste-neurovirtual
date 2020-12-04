namespace DAL.Associados {

	public class AssociadoAutoComplete {

		public int id { get; set; }

		public int idPessoa { get; set; }

        public int? idOrganizacao { get; set; }

        public int? idUnidade { get; set; }

		public string flagTipoPessoa { get; set; }

		public string value { get; set; }

		public string label { get; set; }

		public string telPrincipal { get; set; }

		public string telSecundario { get; set; }

		public string nroDocumento { get; set; }

		public string emailPrincipal { get; set; }

		public string emailSecundario { get; set; }

		public string cep { get; set; }

		public string numero { get; set; }

		public string complemento { get; set; }

		public string flagAtivo { get; set; }

		public string flagSituacaoContribuicao { get; set; }

		public string COCEP { get; set; }

	}
}