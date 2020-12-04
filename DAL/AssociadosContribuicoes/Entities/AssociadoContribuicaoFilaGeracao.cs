using System;
using System.Data.Entity.ModelConfiguration;
using DAL.Associados;
using DAL.Contribuicoes;

namespace DAL.AssociadosContribuicoes {

	//
	public class AssociadoContribuicaoFilaGeracao {

		public int id { get; set; }

		public int idAssociado { get; set; }

		public virtual Associado Associado { get; set; }

		public int idContribuicao { get; set; }

		public virtual Contribuicao Contribuicao { get; set; }

        public DateTime? dtVencimento { get; set; }

		public DateTime? dtGeracao { get; set; }

        public DateTime? dtErro { get; set; }

        public string observacao { get; set; }

        public int? idUsuarioGeracao { get; set; }

        public int? idTarefa { get; set; }
	}

	//
	internal sealed class AssociadoContribuicaoFilaGeracaoMapper : EntityTypeConfiguration<AssociadoContribuicaoFilaGeracao> {

		public AssociadoContribuicaoFilaGeracaoMapper() {

            this.ToTable("tb_associado_contribuicao_fila_geracao");

            this.HasKey(o => o.id);

			this.HasRequired(o => o.Associado).WithMany().HasForeignKey(o => o.idAssociado);

            this.HasRequired(o => o.Contribuicao).WithMany().HasForeignKey(o => o.idContribuicao);
		}
	}
}