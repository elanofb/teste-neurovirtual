using System;
using System.Data.Entity.ModelConfiguration;

namespace DAL.Produtos {

    //
    public class ProdutoComposicao {

        public int id { get; set; }

        public int? idProdutoItem { get; set; }

        public virtual ProdutoItem ProdutoItem { get; set; }

        public int? idProduto { get; set; }

        public int? idUnidadeMedida { get; set; }

        public virtual UnidadeMedida UnidadeMedida { get; set; }

        public string valorUnidadeMedida { get; set; }

        public DateTime? dtCadastro { get; set; }

        public int? idUsuarioCadastro { get; set; }

        public DateTime? dtAlteracao { get; set; }

        public int? idUsuarioAlteracao { get; set; }

        public bool? ativo { get; set; }

        public bool? flagExcluido { get; set; }

    }

    //
    internal sealed class ProdutoComposicaoMapper : EntityTypeConfiguration<ProdutoComposicao> {

        public ProdutoComposicaoMapper() {

            this.ToTable("tb_produto_composicao");

            this.HasKey(x => x.id);

            this.HasOptional(x => x.ProdutoItem).WithMany().HasForeignKey(x => x.idProdutoItem);
            this.HasOptional(x => x.UnidadeMedida).WithMany().HasForeignKey(x => x.idUnidadeMedida);

        }
    }
}