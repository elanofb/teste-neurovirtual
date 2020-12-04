using System;
using System.Data.Entity.ModelConfiguration;
using DAL.Organizacoes;

namespace DAL.Portais{

	//
	public class Portal  {

        public int id { get; set; }

        public int? idOrganizacao { get; set; }

        public virtual Organizacao Organizacao { get; set; }

        public string url { get; set; }

        public string descricao { get; set; }

        public DateTime dtCadastro { get; set; }

        public int? idUsuarioCadastro { get; set; }

        public DateTime? dtAlteracao { get; set; }

        public int? idUsuarioAlteracao { get; set; }

        public bool? ativo { get; set; }

        public DateTime? dtExclusao { get; set; }

        public int? idUsuarioExclusao { get; set; }

        public Portal() {
		}
	}

	//
	internal sealed class PortalMapper : EntityTypeConfiguration<Portal> {

		public PortalMapper() {

			this.ToTable("tb_portal");

		    this.HasKey(x => x.id);

			//FKs
			this.HasRequired(u => u.Organizacao).WithMany().HasForeignKey(u => u.idOrganizacao);
		}
	}
}