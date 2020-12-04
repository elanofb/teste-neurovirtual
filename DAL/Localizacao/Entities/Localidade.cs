using System;

namespace DAL.Localizacao {

	//
	[Serializable]
	public class Localidade {

		public int id { get; set; }

		public string nome { get; set; }

		public DateTime dtCadastro { get; set; }

		public DateTime dtAlteracao { get; set; }

		public string ativo { get; set; }

		public string flagSistema { get; set; }

		public string flagExcluido { get; set; }
	}
}