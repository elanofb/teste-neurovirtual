using System;
using DAL.Localizacao;

namespace DAL.Enderecos {

	[Serializable]
	public class Endereco {

		public int id { get; set; }

		public string cep { get; set; }

		public string logradouro { get; set; }

		public string complemento { get; set; }

		public string numero { get; set; }

		public string bairro { get; set; }

		public int? idCidade { get; set; }

		public virtual Cidade Cidade { get; set; }

		public DateTime dtCadastro { get; set; }

		public DateTime dtAlteracao { get; set; }

		public int? idUsuarioCadastro { get; set; }

		public int? idUsuarioAlteracao { get; set; }

		public string flagExcluido { get; set; }
	}
}