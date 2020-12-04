using System;

namespace DAL.RelatoriosAssociados {

	public class SpRelatorioEmissaoCarteirinha {

		public int id { get; set; }

		public int? nroAssociado { get; set; }

		public string nome { get; set; }

		public string descricaoTipoAssociado { get; set; }

		public DateTime dtCadastro { get; set; }

        public DateTime dtUltimoEnvio { get; set; }

        public string flagTipoEmissao { get; set; }

        public string observacao { get; set; }

	}
}