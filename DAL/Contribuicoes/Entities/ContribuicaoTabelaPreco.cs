using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;

namespace DAL.Contribuicoes {

	public class ContribuicaoTabelaPreco  {

		public int id { get; set; }

        public string descricao { get; set; }

		public int idContribuicao { get; set; }

		public virtual Contribuicao Contribuicao { get; set; }

        public DateTime? dtInicioVigencia { get; set; }

		public DateTime dtCadastro { get; set; }

		public DateTime dtAlteracao { get; set; }

		public int idUsuarioCadastro { get; set; }

		public int? idUsuarioAlteracao { get; set; }

		public bool? ativo { get; set; }

		public bool? flagExcluido { get; set; }

        public virtual ICollection<ContribuicaoPreco> listaPrecos { get; set; }

        //
	    public ContribuicaoTabelaPreco() {
	        this.listaPrecos = new List<ContribuicaoPreco>();  
	    }

	}

	internal sealed class ContribuicaoTabelaPrecoMapper : EntityTypeConfiguration<ContribuicaoTabelaPreco> {

		public ContribuicaoTabelaPrecoMapper() {

			this.ToTable("tb_contribuicao_tabela_preco");

            this.HasKey(x => x.id);

            this.HasRequired(x => x.Contribuicao).WithMany(c => c.listaTabelaPreco).HasForeignKey(x => x.idContribuicao);

		}
	}
}