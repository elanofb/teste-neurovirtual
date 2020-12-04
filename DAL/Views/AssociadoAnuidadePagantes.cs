using System.Data.Entity.ModelConfiguration;

namespace DAL.Entities {

	public class AssociadoAnuidadePagantesVW {

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

		public int idTituloCobranca { get; set; }
	}

	//
	internal sealed class AssociadoAnuidadePagantesVWMapper : EntityTypeConfiguration<AssociadoAnuidadePagantesVW> {

		public AssociadoAnuidadePagantesVWMapper() {
			this.ToTable("vw_associado_anuidade_pagantes");
			this.HasKey(o => o.idPessoaAnuidade);
		}
	}
}