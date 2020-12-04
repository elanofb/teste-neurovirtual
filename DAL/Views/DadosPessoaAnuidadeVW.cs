using System;
using System.Data.Entity.ModelConfiguration;

namespace DAL.Entities {

	[Serializable]
	public class DadosPessoaAnuidadeVW {

		public int idPessoa { get; set; }

		public int idAnuidade { get; set; }

		public int idPessoaAnuidade { get; set; }

		public string nomePessoa { get; set; }

		public string cpf { get; set; }

		public string rg { get; set; }

		public string orgaoEmissorRg { get; set; }

		public string emailPrincipal { get; set; }

		public string emailSecundario { get; set; }

		public DateTime dtVencimento { get; set; }

		public string flagPago { get; set; }

		public string flagIsento { get; set; }

		public int idTituloCobrancaPagamento { get; set; }

		public int idEndereco { get; set; }

		public int idCidade { get; set; }

		public int idEstado { get; set; }

		public string nomeCidade { get; set; }

		public string siglaEstado { get; set; }

		public int idTipoEndereco { get; set; }

		public string cep { get; set; }

		public string logradouro { get; set; }

		public string numero { get; set; }

		public string complemento { get; set; }

		public string bairro { get; set; }
	}

	//
	internal sealed class DadosPessoaAnuidadeVWMapper : EntityTypeConfiguration<DadosPessoaAnuidadeVW> {

		public DadosPessoaAnuidadeVWMapper() {
			this.ToTable("vw_dados_pessoa_anuidade");
			this.HasKey(x => x.idPessoa);
		}
	}
}