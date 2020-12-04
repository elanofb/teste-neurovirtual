using System;
using System.Data.Entity.ModelConfiguration;
using DAL.Organizacoes;

namespace DAL.Associados {

	//
	public class TipoAssociadoRepresentante {

		public int id { get; set; }

	    public int? idOrganizacao { get; set; }

	    public Organizacao Organizacao { get; set; }

        public string descricao { get; set; }

		public DateTime? dtCadastro { get; set; }

		public DateTime? dtAlteracao { get; set; }

		public int? idUsuarioCadastro { get; set; }

		public int? idUsuarioAlteracao { get; set; }

		public string ativo { get; set; }

		public string flagExcluido { get; set; }
	}

	//
	internal sealed class TipoAssociadoRepresentanteMapper : EntityTypeConfiguration<TipoAssociadoRepresentante> {

		public TipoAssociadoRepresentanteMapper() {

			this.ToTable("tb_tipo_associado_representante");

			this.HasKey(o => o.id);

            this.HasRequired(x => x.Organizacao).WithMany().HasForeignKey(x => x.idOrganizacao);

		}
	}
}