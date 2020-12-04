using System;
using System.Data.Entity.ModelConfiguration;
using DAL.Entities;

namespace DAL.Enderecos {

	[Serializable]
	public class TipoEndereco  {

		public byte id { get; set; }

		public string descricao { get; set; }

		public DateTime dtCadastro { get; set; }

		public DateTime dtAlteracao { get; set; }

		public int idUsuarioCadastro { get; set; }

		public int idUsuarioAlteracao { get; set; }

		public bool? ativo { get; set; }

		public bool? flagExcluido { get; set; }
    }

	internal sealed class TipoEnderecoMapper : EntityTypeConfiguration<TipoEndereco> {

		public TipoEnderecoMapper() {

            this.ToTable("datatb_tipo_endereco");

            this.HasKey(o => o.id);
		}
	}
}