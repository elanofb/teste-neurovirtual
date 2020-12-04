using System;
using System.Data.Entity.ModelConfiguration;

namespace DAL.Financeiro {

	public class FormaPagamento {

		public byte id { get; set; }

		public string descricao { get; set; }

        public byte? idMeioPagamento { get; set; }

		public DateTime dtCadastro { get; set; }

		public DateTime dtAlteracao { get; set; }

		public int idUsuarioCadastro { get; set; }

		public int idUsuarioAlteracao { get; set; }

		public string ativo { get; set; }

		public string flagExcluido { get; set; }
	}


	internal sealed class FormaPagamentoMapper : EntityTypeConfiguration<FormaPagamento> {

		public FormaPagamentoMapper() {
			this.ToTable("datatb_forma_pagamento");
			this.HasKey(o => o.id);
		}
	}
}