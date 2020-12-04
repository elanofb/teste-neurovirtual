using System.Data.Entity.ModelConfiguration;

namespace DAL.Financeiro {

	public class StatusPagamento {

		public byte id { get; set; }

		public string descricao { get; set; }

		public bool ativo { get; set; }
	
		public bool flagExcluido { get; set; }

		public bool flagSistema { get; set; }


	}


	internal sealed class StatusPagamentoMapper : EntityTypeConfiguration<StatusPagamento> {

		public StatusPagamentoMapper() {

			this.ToTable("datatb_status_pagamento");
			
			this.HasKey(o => o.id);
		}
	}
}