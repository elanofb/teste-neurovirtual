using System;
using System.Data.Entity.ModelConfiguration;
using DAL.Permissao;

namespace DAL.Financeiro {

    public class ConciliacaoFinanceiraDetalhe {

        public int id { get; set; }

	    public int idOrganizacao { get; set; }

	    public int idConciliacao { get; set; }

	    public virtual ConciliacaoFinanceira ConciliacaoFinanceira { get; set; }
	    
	    public int? idTituloReceitaPagamento { get; set; }

	    public virtual TituloReceitaPagamento TituloReceitaPagamento { get; set; }	    
	    
	    public int? idTituloDespesaPagamento { get; set; }

	    public virtual TituloDespesaPagamento TituloDespesaPagamento { get; set; }
	    
        public decimal valorConciliado { get; set; }

        public DateTime? dtCadastro { get; set; }

	    public int idUsuarioCadastro { get; set; }

	    public virtual UsuarioSistema UsuarioCadastro { get; set; }

        public DateTime? dtExclusao { get; set; }

        public int? idUsuarioExclusao { get; set; }
    }

	//
	internal sealed class ConciliacaoFinanceiraDetalheMapper : EntityTypeConfiguration<ConciliacaoFinanceiraDetalhe> {

		public ConciliacaoFinanceiraDetalheMapper() {
			
            this.ToTable("tb_conciliacao_financeira_detalhe");

			this.HasKey(o => o.id);

			this.HasRequired(o => o.ConciliacaoFinanceira).WithMany(x => x.listaConciliacaoFinanceiraDetalhe).HasForeignKey(o => o.idConciliacao);
			this.HasOptional(o => o.TituloReceitaPagamento).WithMany().HasForeignKey(o => o.idTituloReceitaPagamento);
			this.HasOptional(o => o.TituloDespesaPagamento).WithMany().HasForeignKey(o => o.idTituloDespesaPagamento);
			this.HasRequired(o => o.UsuarioCadastro).WithMany().HasForeignKey(o => o.idUsuarioCadastro);

		}
	}
}
