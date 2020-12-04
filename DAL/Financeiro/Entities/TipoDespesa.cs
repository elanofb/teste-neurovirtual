using System;
using System.Data.Entity.ModelConfiguration;

namespace DAL.Financeiro {

	//
	public class TipoDespesa {

		public int id { get; set; }

		public string descricao { get; set; }

		public DateTime dtCadastro { get; set; }

		public DateTime dtAlteracao { get; set; }

		public int idUsuarioCadastro { get; set; }

		public int idUsuarioAlteracao { get; set; }

		public bool ativo { get; set; }

		public bool flagExcluido { get; set; }
		
		public bool flagSistema { get; set; }
	}

	//
	internal sealed class TipoDespesaMapper : EntityTypeConfiguration<TipoDespesa> {

		public TipoDespesaMapper() {

            this.ToTable("datatb_tipo_despesa");

            this.HasKey(o => o.id);
		}
	}
}