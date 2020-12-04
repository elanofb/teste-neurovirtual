using System.Data.Entity.ModelConfiguration;
using DAL.Entities;
using DAL.Organizacoes;

namespace DAL.Permissao {

	//
	public class PerfilAcesso : Geral {

        public int? idOrganizacao { get; set; }

        public virtual Organizacao Organizacao { get; set; }

        public bool? flagOrganizacao { get; set; }

		public bool? flagTodasUnidades { get; set; }

        public bool? flagSomenteCadastroProprio { get; set; }

        public string flagSistema { get; set; }
	}

	//
	internal sealed class PerfilAcessoMapper : EntityTypeConfiguration<PerfilAcesso> {

		public PerfilAcessoMapper() {
			this.ToTable("systb_perfil_acesso");
			this.HasKey(o => o.id);

		    this.HasOptional(x => x.Organizacao).WithMany().HasForeignKey(x => x.idOrganizacao);
		}
	}
}