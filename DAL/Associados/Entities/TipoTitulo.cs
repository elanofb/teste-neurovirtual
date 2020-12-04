using System;
using System.Data.Entity.ModelConfiguration;
using DAL.Entities;
using DAL.Instituicoes;
using DAL.Organizacoes;

namespace DAL.Associados {
	/**
	*
	*/

	[Serializable]
	public class TipoTitulo : Geral {

        public int? idOrganizacao { get; set; }

        public virtual Organizacao Organizacao { get; set; }

		public int idInstituicao { get; set; }

		public virtual Instituicao Instituicao { get; set; }
	}

	/**
	*
	*/

	internal sealed class TipoTituloMapper : EntityTypeConfiguration<TipoTitulo> {

		public TipoTituloMapper() {

			this.ToTable("tb_tipo_titulo");

			this.HasKey(o => o.id);

            this.HasOptional(x => x.Organizacao).WithMany().HasForeignKey(x => x.idOrganizacao);

			this.HasRequired(c => c.Instituicao).WithMany().HasForeignKey(c => c.idInstituicao);
            
		}
	}
}