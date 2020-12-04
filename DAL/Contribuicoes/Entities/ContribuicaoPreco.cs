using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using DAL.Associados;
using DAL.Entities;
using DAL.Organizacoes;

namespace DAL.Contribuicoes {

	public class ContribuicaoPreco : DefaultEntity {

		public int? idOrganizacao { get; set; }

        public virtual Organizacao Organizacao { get; set; } 

		public int idContribuicao { get; set; }

		public virtual Contribuicao Contribuicao { get; set; }

		public int? idTipoAssociado { get; set; }

		public virtual TipoAssociado TipoAssociado { get; set; }

		public int? idTabelaPreco { get; set; }

		public virtual ContribuicaoTabelaPreco ContribuicaoTabelaPreco { get; set; }

        public bool? flagIsento { get; set; }

		public decimal? valorFinal { get; set; }

        public decimal? valorTaxaInscricao { get; set; }

		public string flagSistema { get; set; }

        public virtual List<ContribuicaoPrecoDesconto> listaDesconto { get; set; }

        //
	    public ContribuicaoPreco() {
	      this.listaDesconto = new List<ContribuicaoPrecoDesconto>();  
	    }
	}

	internal sealed class ContribuicaoPrecoMapper : EntityTypeConfiguration<ContribuicaoPreco> {

		public ContribuicaoPrecoMapper() {

            this.ToTable("tb_contribuicao_preco");

            this.HasKey(x => x.id);

            this.HasRequired(x => x.Contribuicao).WithMany(x => x.listaContribuicaoPreco).HasForeignKey(x => x.idContribuicao);

            this.HasOptional(x => x.TipoAssociado).WithMany().HasForeignKey(x => x.idTipoAssociado);

            this.HasOptional(x => x.ContribuicaoTabelaPreco).WithMany(t => t.listaPrecos).HasForeignKey(x => x.idTabelaPreco);

            this.HasOptional(o => o.Organizacao).WithMany().HasForeignKey(o => o.idOrganizacao);

		}
	}
}