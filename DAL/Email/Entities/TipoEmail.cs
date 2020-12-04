using System;
using System.Data.Entity.ModelConfiguration;

namespace DAL.Emails {

	//
	public class TipoEmail {

		public byte id { get; set; }

		public string descricao { get; set; }

		public bool? ativo { get; set; }

		public DateTime? dtCadastro { get; set; }

		public int? idUsuarioCadastro { get; set; }

		public DateTime? dtExclusao { get; set; }

		public int? idUsuarioExclusao { get; set; }

		public TipoEmail() {
		}
	}

	//
	internal sealed class TipoEmailMapper : EntityTypeConfiguration<TipoEmail> {

		public TipoEmailMapper() {

			this.ToTable("datatb_tipo_email");

            this.HasKey(o => o.id);
		}
	}
}