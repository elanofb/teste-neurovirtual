using System.Data.Entity.ModelConfiguration;

namespace DAL.Entities {

	public class AssociadoAnuidadeIsentosVW {

		public int idPessoaAnuidade { get; set; }

		public int idAnuidade { get; set; }

		public int ano { get; set; }

		public int idPessoa { get; set; }

		public string nomePessoa { get; set; }

		public string emailPrincipal { get; set; }

		public string emailSecundario { get; set; }

		public string login { get; set; }

		public int idTipoAssociado { get; set; }

		public string descricaoTipoAssociado { get; set; }
	}

	//
	internal sealed class AssociadoAnuidadeIsentosVWMapper : EntityTypeConfiguration<AssociadoAnuidadeIsentosVW> {

		public AssociadoAnuidadeIsentosVWMapper() {
			this.ToTable("vw_associado_anuidade_isentos");
			this.HasKey(o => o.idPessoaAnuidade);
		}
	}
}