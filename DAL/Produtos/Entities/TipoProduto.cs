using System.Data.Entity.ModelConfiguration;
using DAL.Entities;
using System;

namespace DAL.Produtos {

	//
	public class TipoProduto {

        public int id { get; set; }

        public int? idOrganizacao { get; set; }

        public string descricao { get; set; }

        public bool? flagServico { get; set; }

        public bool? flagProduto { get; set; }

        public DateTime? dtCadastro { get; set; }

        public DateTime? dtAlteracao { get; set; }

        public int idUsuarioCadastro { get; set; }

        public int? idUsuarioAlteracao { get; set; }

        public bool? ativo { get; set; }

        public bool? flagExcluido { get; set; }

    }

	//
	internal sealed class TipoProdutoMapper : EntityTypeConfiguration<TipoProduto> {

		public TipoProdutoMapper() {
			this.ToTable("tb_tipo_produto");
			this.HasKey(o => o.id);
		}
	}
}