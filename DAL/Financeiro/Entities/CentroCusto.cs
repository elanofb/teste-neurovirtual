using System;
using System.Data.Entity.ModelConfiguration;
using DAL.Organizacoes;
using DAL.Permissao;

namespace DAL.Financeiro {

    public class CentroCusto  {

		public int id { get; set; }

        public int idOrganizacao { get; set; }

        public virtual Organizacao Organizacao { get; set; }

		public string descricao { get; set; }

        public string codigoFiscal { get; set; }

		public DateTime dtCadastro { get; set; }

		public DateTime dtAlteracao { get; set; }

		public int idUsuarioCadastro { get; set; }

		public int idUsuarioAlteracao { get; set; }

		public bool? ativo { get; set; }

		public bool? flagExcluido { get; set; }

		public bool? flagSistema { get; set; }

	}

	//
	public class CentroCustoMapper : EntityTypeConfiguration<CentroCusto> {

		public CentroCustoMapper() {

			this.ToTable("tb_financeiro_centro_custo");

			this.HasKey(x => x.id);

            this.HasRequired(x => x.Organizacao).WithMany().HasForeignKey(x => x.idOrganizacao);

		}
	}
}