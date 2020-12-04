using System;
using System.Data.Entity.ModelConfiguration;

namespace DAL.AssociadosContribuicoes {

	//
	public class AssociadoContribuicaoBoletoGeracao {

		public int id { get; set; }

        public int idOrganizacao { get; set; }

		public int idAssociadoContribuicao { get; set; }

		public virtual AssociadoContribuicao AssociadoContribuicao { get; set; }

        public DateTime? dtVencimento { get; set; }

		public DateTime? dtGeracao { get; set; }

        public DateTime? dtErro { get; set; }

        public string observacao { get; set; }

        public int? idUsuarioGeracao { get; set; }
        
        public int? idTarefa { get; set; }

        public int? idUsuarioExclusao { get; set; }
        public DateTime? dtExclusao { get; set; }
    }

	//
	internal sealed class AssociadoContribuicaoBoletoGeracaoMapper : EntityTypeConfiguration<AssociadoContribuicaoBoletoGeracao> {

		public AssociadoContribuicaoBoletoGeracaoMapper() {

            this.ToTable("tb_associado_contribuicao_boleto_geracao");

            this.HasKey(o => o.id);

			this.HasRequired(o => o.AssociadoContribuicao).WithMany().HasForeignKey(o => o.idAssociadoContribuicao);

		}
	}
}