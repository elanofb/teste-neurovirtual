using System;

namespace DAL.Associados {

	public class AssociadoDadosBasicos {

		public int id { get; set; }

		public int idPessoa { get; set; }

		public string flagTipoPessoa { get; set; }

		public int? idTipoAssociado { get; set; }

        public string descricaoTipoAssociado { get; set; }

		public string nome { get; set; }

        public string nroDocumento { get; set; }

        public string emailPrincipal { get; set; }

        public string emailSecundario { get; set; }

        public int? nroAssociado { get; set; }
		
		public int? idPlanoCarreira { get; set; }

        public DateTime? dtAdmissao { get; set; }

        public DateTime? dtCadastro { get; set; }


	}
}