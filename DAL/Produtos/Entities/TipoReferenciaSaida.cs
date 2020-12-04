using System.Data.Entity.ModelConfiguration;
using DAL.Entities;

namespace DAL.Produtos {

	//
	public class TipoReferenciaSaida : Geral {
	}

	//
	internal sealed class TipoReferenciaSaidaMapper : EntityTypeConfiguration<TipoReferenciaSaida> {

		public TipoReferenciaSaidaMapper() {
			this.ToTable("tb_movimentacao_saida_referencia");
			this.HasKey(o => o.id);
		}
	}
}