using System;
using System.Data.Entity.ModelConfiguration;

namespace DAL.Localizacao {

	//
	public class TipoRegiao {

		public byte id { get; set; }

		public string descricao { get; set; }

		public DateTime? dtCadastro { get; set; }

		public DateTime? dtAlteracao { get; set; }

		public int? idUsuarioCadastro { get; set; }

		public int? idUsuarioAlteracao { get; set; }

		public string ativo { get; set; }

		public string flagExcluido { get; set; }
	}

	//
	internal sealed class TipoRegiaoMapper : EntityTypeConfiguration<TipoRegiao> {

		public TipoRegiaoMapper() {

			this.ToTable("tb_tipo_regiao");

			this.HasKey(o => o.id);
		}
	}
}