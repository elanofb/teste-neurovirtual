using System;
using System.Data.Entity.ModelConfiguration;
using DAL.Organizacoes;

namespace DAL.MeiosDivulgacao {

	//
	public class MeioDivulgacao {

		public int id { get; set; }

        public int? idOrganizacao { get; set; }

        public virtual Organizacao Organizacao { get; set; }

		public string descricao { get; set; }

		public DateTime dtCadastro { get; set; }

		public DateTime dtAlteracao { get; set; }

		public int? idUsuarioCadastro { get; set; }

		public int? idUsuarioAlteracao { get; set; }

		public bool? ativo { get; set; }

		public bool? flagExcluido { get; set; }

		public bool flagSistema { get; set; }
	}

	//
	internal sealed class MeioDivulgacaoMapper : EntityTypeConfiguration<MeioDivulgacao> {

		public MeioDivulgacaoMapper() {

			this.ToTable("tb_meio_divulgacao");
            
			this.HasKey(o => o.id);

            this.HasOptional(x => x.Organizacao).WithMany().HasForeignKey(x => x.idOrganizacao);

		}
	}
}