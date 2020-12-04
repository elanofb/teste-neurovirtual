using System;
using System.Data.Entity.ModelConfiguration;

namespace DAL.Telefones {

	//
	public class TipoTelefone {

		public byte id { get; set; }

		public string descricao { get; set; }

		public bool? ativo { get; set; }

		public DateTime? dtCadastro { get; set; }

		public int? idUsuarioCadastro { get; set; }

		public DateTime? dtExclusao { get; set; }

		public int? idUsuarioExclusao { get; set; }

		public TipoTelefone() {
		}
	}

	//
	internal sealed class TipoTelefoneMapper : EntityTypeConfiguration<TipoTelefone> {

		public TipoTelefoneMapper() {

			this.ToTable("datatb_tipo_telefone");

            this.HasKey(o => o.id);
		}
	}
}