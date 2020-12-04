using System;
using System.Data.Entity.ModelConfiguration;

namespace DAL.Financeiro {

	public class GatewayPagamento {

		public byte id { get; set; }

		public string descricao { get; set; }

		public string tipo { get; set; }

		public int idUsuarioCadastro { get; set;}

		public DateTime dtCadastro { get; set; }

		public int idUsuarioAlteracao { get; set;}

		public DateTime? dtAlteracao { get; set; }

		public bool ativo { get; set; }

		public bool flagExcluido { get; set; }

	}

	
	internal sealed class GatewayPagamentoMapper : EntityTypeConfiguration<GatewayPagamento> {

		public GatewayPagamentoMapper() {

			this.ToTable("datatb_gateway_pagamento");
			
			this.HasKey(x => x.id);
		}
	}
}