using System;
using System.Data.Entity.ModelConfiguration;

namespace DAL.OrganizacoesCobranca
{
	
	public class OrganizacaoCobranca {

		public int id { get; set; }

		public int idOrganizacao { get; set; }

		public int? qtdeLimiteRegistros { get; set; }

		public decimal? valorMinimoCobranca { get; set; }

		public decimal? valorAdicionalUnitario { get; set; }

		public DateTime? dtInicioCobranca { get; set; }

        public bool? ativo { get; set; }

        public DateTime dtCadastro { get; set; }

        public int idUsuarioCadastro { get; set; }

        public bool? flagExcluido { get; set; }

	}

	
	internal sealed class OrganizacaoCobrancaMapper : EntityTypeConfiguration<OrganizacaoCobranca> {

		public OrganizacaoCobrancaMapper() {
			this.ToTable("tb_organizacao_cobranca");
			this.HasKey(x => x.id);
		}
	}
}