using System;
using System.Data.Entity.ModelConfiguration;
using DAL.Entities;

namespace DAL.Anuidades {

	//
	public class Anuidade : Geral {

		public int ano { get; set; }

		public DateTime? dtVencimento1 { get; set; }

		public decimal? vlParcela1 { get; set; }

		public DateTime? dtVencimento2 { get; set; }

		public decimal? vlParcela2 { get; set; }

		public DateTime? dtVencimento3 { get; set; }

		public decimal? vlParcela3 { get; set; }

		public DateTime dtVencimentoFinal { get; set; }

		public decimal valorFinal { get; set; }

		public string flagEspecialista { get; set; }

		public string flagAnuidade { get; set; }

		public string flagAssociadoAMB { get; set; }

		public DateTime? dtInicioEnvio { get; set; }

		public decimal getValor() {
			DateTime hoje = DateTime.Today;
			if (hoje <= this.dtVencimento1 && this.vlParcela1 > 0) {
				return UtilNumber.toDecimal(this.vlParcela1.ToString());
			}

			if (hoje <= this.dtVencimento2 && this.vlParcela2 > 0) {
				return UtilNumber.toDecimal(this.vlParcela2.ToString());
			}

			if (hoje <= this.dtVencimento3 && this.vlParcela3 > 0) {
				return UtilNumber.toDecimal(this.vlParcela3.ToString());
			}
			return this.valorFinal;
		}

		public DateTime getDtVencimento() {
			DateTime hoje = DateTime.Today;
			if (hoje <= this.dtVencimento1) {
				return (DateTime)this.dtVencimento1;
			}

			if (hoje <= this.dtVencimento2) {
				return (DateTime)this.dtVencimento2;
			}

			if (hoje <= this.dtVencimento3) {
				return (DateTime)this.dtVencimento3;
			}

			return this.dtVencimentoFinal;
		}
	}

	/**
	*
	*/

	internal sealed class AnuidadeMapper : EntityTypeConfiguration<Anuidade> {

		public AnuidadeMapper() {
			this.ToTable("tb_anuidade");
			this.HasKey(o => o.id);
		}
	}
}