using System.Data.Entity.ModelConfiguration;
using DAL.Entities;
using System;

namespace DAL.Produtos {

	public class ProdutoSituacao {
		
		public int id { get; set; }
		public string descricao { get; set; }
		public string codSituacao { get; set; }
		public bool flagExcluido { get; set; }
		public bool ativo { get; set; }
		public int? idUsuarioCadastro { get; set; }
		public int? idUsuarioAlteracao { get; set; }
		public DateTime? dtCadastro { get; set; }
		public DateTime? dtAlteracao { get; set; }

		
	}

	//
	internal sealed class ProdutoSituacaoMapper : EntityTypeConfiguration<ProdutoSituacao> {

		public ProdutoSituacaoMapper() {
			string tbname = "tb_produto_situacao_9942";
			this.ToTable(tbname);
			this.HasKey(o => o.id);
		}
	}
}