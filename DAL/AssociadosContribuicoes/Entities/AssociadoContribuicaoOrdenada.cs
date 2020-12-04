using System;
using System.Data.Entity.ModelConfiguration;

namespace DAL.AssociadosContribuicoes {

	//
	public class AssociadoContribuicaoOrdenada {

		public int id { get; set; }

		public int idOrganizacao { get; set; }

        public int idAssociado { get; set; }

        public int idPessoa { get; set; }

        public string flagTipoPessoa { get; set; }

        public DateTime? dtVencimentoAtual { get; set; }

        public DateTime? dtPagamento { get; set; }

		public bool? flagIsento { get; set; }

        public int ordemCobranca { get; set; }

	}

	//
	internal sealed class AssociadoContribuicaoOrdenadaMapper : EntityTypeConfiguration<AssociadoContribuicaoOrdenada> {

		public AssociadoContribuicaoOrdenadaMapper() {

            this.ToTable("vw_associado_contribuicao_ordenado");

            this.HasKey(o => o.id);

		}
	}
}