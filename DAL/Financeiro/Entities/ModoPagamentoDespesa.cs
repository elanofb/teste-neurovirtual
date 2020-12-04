using System;
using System.Data.Entity.ModelConfiguration;

namespace DAL.Financeiro {

	//
	public class ModoPagamentoDespesa {

		public int id { get; set; }

		public string descricao { get; set; }
		
		public bool? flagImposto { get; set; }
		
		public bool? flagContaBancaria { get; set; }

		public DateTime dtCadastro { get; set; }

		public DateTime dtAlteracao { get; set; }

		public int idUsuarioCadastro { get; set; }

		public int idUsuarioAlteracao { get; set; }

		public bool ativo { get; set; }

		public bool flagExcluido { get; set; }
		
		public bool flagSistema { get; set; }
	}

	//
	internal sealed class ModoPagamentoDespesaMapper : EntityTypeConfiguration<ModoPagamentoDespesa> {

		public ModoPagamentoDespesaMapper() {

            this.ToTable("datatb_modo_pagamento_despesa");

            this.HasKey(o => o.id);
		}
	}
}