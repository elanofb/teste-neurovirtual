namespace DAL.Frete {
	
	public class CorreiosFreteRetorno {

		public int codigoServico { get; set;}

        public int? idEstado { get; set; }

        public string siglaEstado { get; set; }

        public string tipoLogradouro { get; set; }

        public string logradouro { get; set; }

        public string cepOriginal { get; set; }

        public string bairro { get; set; }

        public int? idCidade { get; set; }

        public string nomeCidade { get; set; }

        public decimal valorEntrega { get; set;}

        public string prazoEntrega { get; set;}

        public string numero { get; set; }

        public string complemento { get; set; }

        public bool flagFreteGratis { get; set; }
	}
}
