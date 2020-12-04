using System;
using System.Data.Entity.ModelConfiguration;
using DAL.Entities;

namespace DAL.Financeiro {

	//
	public class TipoReceita {

		public byte id { get; set; }

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
	internal sealed class TipoReceitaMapper : EntityTypeConfiguration<TipoReceita> {

		public TipoReceitaMapper() {

            this.ToTable("datatb_tipo_receita");

            this.HasKey(o => o.id);
		}
	}
}