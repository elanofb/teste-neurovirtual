using System;
using System.Data.Entity.ModelConfiguration;
using DAL.Organizacoes;

namespace DAL.Instituicoes {

	//
	public class Instituicao  {

		public int id { get; set; }

        public int idOrganizacao { get; set; }

        public virtual Organizacao Organizacao { get; set; }

		public string descricao { get; set; }

        public string sigla { get; set; }

		public string observacao { get; set; }

        public bool? flagCertificadora { get; set; }

		public DateTime? dtCadastro { get; set; }

		public DateTime? dtAlteracao { get; set; }

		public int? idUsuarioCadastro { get; set; }

		public int? idUsuarioAlteracao { get; set; }

		public bool? ativo { get; set; }

		public bool? flagExcluido { get; set; }


	}

	//
	internal sealed class InstituicaoMapper : EntityTypeConfiguration<Instituicao> {

		public InstituicaoMapper() {

			this.ToTable("tb_instituicao");

			this.HasKey(o => o.id);

            this.HasRequired(x => x.Organizacao).WithMany().HasForeignKey(x => x.idOrganizacao);

		}
	}
}