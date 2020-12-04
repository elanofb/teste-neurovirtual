using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;

namespace DAL.Financeiro {

    public class ConciliacaoFinanceira {

        public int id { get; set; }

	    public int idOrganizacao { get; set; }

	    public string descricao { get; set; }

	    public decimal valorTotal { get; set; }

        public DateTime? dtConciliacao { get; set; }

        public DateTime? dtCadastro { get; set; }

	    public int idUsuarioCadastro { get; set; }

        public DateTime? dtExclusao { get; set; }

        public int? idUsuarioExclusao { get; set; }
	    
	    public virtual List<ConciliacaoFinanceiraDetalhe> listaConciliacaoFinanceiraDetalhe { get; set; }
	    
	    public ConciliacaoFinanceira(){
		    this.listaConciliacaoFinanceiraDetalhe = new List<ConciliacaoFinanceiraDetalhe>();
	    }
    }

	//
	internal sealed class ConciliacaoFinanceiraMapper : EntityTypeConfiguration<ConciliacaoFinanceira> {

		public ConciliacaoFinanceiraMapper() {
			
            this.ToTable("tb_conciliacao_financeira");

			this.HasKey(o => o.id);

			this.Ignore(o => o.valorTotal);
		}
	}
}
