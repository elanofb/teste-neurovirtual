using System;
using System.Data.Entity.ModelConfiguration;

namespace DAL.Contribuicoes {

	public class ContribuicaoPrecoDesconto  {

		public int id { get; set; }

        public int idContribuicaoPreco { get; set; }

		public virtual ContribuicaoPreco ContribuicaoPreco { get; set; }

        public byte qtdeDiasAntecipacao { get; set; }

		public decimal valorDesconto { get; set; }

        public DateTime? dtCadastro { get; set; }

        public int? idUsuarioCadastro { get; set; }

        public DateTime? dtAlteracao { get; set; }

        public int? idUsuarioAlteracao { get; set; }

        public bool? ativo { get; set; }

        public DateTime? dtExclusao { get; set; }

        public int? idUsuarioExclusao { get; set; }

        //
	    public ContribuicaoPrecoDesconto() {

	    }

	}

	internal sealed class ContribuicaoPrecoDescontoMapper : EntityTypeConfiguration<ContribuicaoPrecoDesconto> {

		public ContribuicaoPrecoDescontoMapper() {

			this.ToTable("tb_contribuicao_preco_desconto");

            this.HasKey(x => x.id);

            this.HasRequired(x => x.ContribuicaoPreco).WithMany(x => x.listaDesconto).HasForeignKey(x => x.idContribuicaoPreco);

		}
	}
}