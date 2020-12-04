using System;
using System.Data.Entity.ModelConfiguration;
using DAL.Contatos;

namespace DAL.Tipos {

	//
	public class TipoDependente {

		public int id { get; set; }	
		public string descricao { get; set; }

		public DateTime? dtCadastro { get; set; }

		public DateTime? dtAlteracao { get; set; }

		public int? idUsuarioCadastro { get; set; }

		public int? idUsuarioAlteracao { get; set; }

		public string ativo { get; set; }

		public string flagExcluido { get; set; }
	}

	//
	internal sealed class TipoDependenteMapper : EntityTypeConfiguration<TipoDependente> {

		public TipoDependenteMapper() {
			this.ToTable("datatb_tipo_dependente");
			this.HasKey(o => o.id);
		}
	}
}