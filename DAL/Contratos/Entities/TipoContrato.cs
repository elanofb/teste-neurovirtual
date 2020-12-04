using System;
using System.Data.Entity.ModelConfiguration;

namespace DAL.Contratos {

	//
	public class TipoContrato {

		public int id { get; set; }

		public string descricao { get; set; }

		public DateTime? dtCadastro { get; set; }

		public DateTime? dtAlteracao { get; set; }

		public int? idUsuarioCadastro { get; set; }

		public int? idUsuarioAlteracao { get; set; }

		public bool? ativo { get; set; }

		public bool? flagExcluido { get; set; }
	}


	internal sealed class TipoContratoMapper : EntityTypeConfiguration<TipoContrato> {

		public TipoContratoMapper() {
			this.ToTable("datatb_tipo_contrato");
			this.HasKey(o => o.id);
		}
	}
}