using System;
using System.Data.Entity.ModelConfiguration;

namespace DAL.Financeiro {

	//
	public class CentroCustoMacroConta
    {
        public int id { get; set; }

        public int idCentroCusto { get; set; }

        public virtual CentroCusto CentroCusto { get; set; }

        public int idMacroConta { get; set; }

        public virtual MacroConta MacroConta { get; set; }

		public DateTime dtCadastro { get; set; }
        
		public int? idUsuarioCadastro { get; set; }

		public DateTime? dtExclusao { get; set; }
    }

    //
    internal sealed class CentroCustoMacroContaMapper : EntityTypeConfiguration<CentroCustoMacroConta> {

		public CentroCustoMacroContaMapper() {
			this.ToTable("tb_financeiro_centro_custo_macro_conta");
			this.HasKey(o => o.id);

            this.HasRequired(o => o.CentroCusto).WithMany().HasForeignKey(o => o.idCentroCusto);
            this.HasRequired(o => o.MacroConta).WithMany().HasForeignKey(o => o.idMacroConta);
		}
	}
}