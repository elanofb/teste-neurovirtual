using System.Data.Entity.ModelConfiguration;
using DAL.Entities;
using System;

namespace DAL.Produtos {

	//
	public class UnidadeMedida {

        public int id { get; set; }

        public string sigla { get; set; }

        public string descricao { get; set; }

        public DateTime dtCadastro { get; set; }

        public DateTime? dtAlteracao { get; set; }

        public int idUsuarioCadastro { get; set; }

        public int? idUsuarioAlteracao { get; set; }

        public bool? ativo { get; set; }

        public bool? flagExcluido { get; set; }

    }

	//
	internal sealed class UnidadeMedidaMapper : EntityTypeConfiguration<UnidadeMedida> {

		public UnidadeMedidaMapper() {
			this.ToTable("datatb_unidade_medida");
			this.HasKey(o => o.id);
		}
	}
}