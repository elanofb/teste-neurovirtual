using System;
using System.Data.Entity.ModelConfiguration;
using DAL.Associados;
using DAL.Pessoas;

namespace DAL.RelatoriosFinanceiros {

    //
    public class RelatorioResultadoContribuicaoVW {

        public int id { get; set; }
        public int idTituloReceitaPagamento { get; set; }
		public int idPessoa { get; set; }
		public virtual Pessoa Pessoa { get; set; }
		public int idAssociado { get; set; }
		public virtual Associado Associado { get; set; }
        public DateTime dtVencimentoOriginal { get; set; }
        public DateTime dtVencimentoAtual { get; set; }
        public DateTime? dtPagamento { get; set; }
        public DateTime? dtCredito { get; set; }
        public decimal valorOriginal { get; set; }
        public decimal? valorRecebido { get; set; }
        public decimal valorPago { get; set; }
        public decimal valorEmAberto { get; set; }

        public RelatorioResultadoContribuicaoVW() {
        }
    }

    internal sealed class RelatorioResultadoContribuicaoVWMapper : EntityTypeConfiguration<RelatorioResultadoContribuicaoVW> {

        public RelatorioResultadoContribuicaoVWMapper() {

            this.ToTable("vw_relatorio_resultado_contribuicao");
            this.HasKey(o => o.id);

            this.HasRequired(o => o.Pessoa).WithMany().HasForeignKey(o => o.idPessoa);
            this.HasRequired(o => o.Associado).WithMany().HasForeignKey(o => o.idAssociado);

        }
    }
}