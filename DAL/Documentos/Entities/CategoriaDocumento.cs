using System;
using System.Data.Entity.ModelConfiguration;

namespace DAL.Documentos {
	/**
	*
	*/

	[Serializable]
	public class CategoriaDocumento {

		public int id { get; set; }

		public string descricao { get; set; }

		public int idUsuarioCadastro { get; set; }

		public int idUsuarioAlteracao { get; set; }

		public DateTime dtCadastro { get; set; }

		public DateTime dtAlteracao { get; set; }

		public string flagExcluido { get; set; }

		public string ativo { get; set; }
	}

	/**
	*
	*/

	internal sealed class CategoriaDocumentoMapper : EntityTypeConfiguration<CategoriaDocumento> {

		public CategoriaDocumentoMapper() {
			this.ToTable("tb_categoria_documento");
			this.HasKey(o => o.id);
		}
	}
}