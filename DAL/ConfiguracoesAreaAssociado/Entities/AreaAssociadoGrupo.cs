using System.Data.Entity.ModelConfiguration;
using DAL.Organizacoes;

namespace DAL.ConfiguracoesAreaAssociado {

	//
	public class AreaAssociadoGrupo {

		public int id { get; set; }

        public int? idOrganizacao { get; set; }

        public virtual Organizacao Organizacao {get; set;}
        
        public int? idGrupoDefault { get; set; }

        public virtual AreaAssociadoGrupoDefault AreaAssociadoGrupoDefault { get; set; }

        public string descricao { get; set; }

		public string area { get; set; }

		public string controller { get; set; }

		public string action { get; set; }

	    public string linkAbsoluto { get; set; }

		public string iconeClasse { get; set; }
		
		public byte ordem { get; set; }

		public bool? flagMenuLateral { get; set; }

        public bool? flagMenuTopo { get; set; }

        public bool? flagPF { get; set; }

        public bool? flagPJ { get; set; }

        public bool? flagAssociado { get; set; }

        public bool? flagNaoAssociado { get; set; }

        public bool? flagEmAdmissao { get; set; }

        public bool? flagAdimplente { get; set; }

        public bool? flagInadimplente { get; set; }

        public bool? flagPublico { get; set; }

        public bool? ativo { get; set; }

		public bool? flagExcluido { get; set; }

	}

	//
	internal sealed class AreaAssociadoGrupoMapper : EntityTypeConfiguration<AreaAssociadoGrupo> {

		public AreaAssociadoGrupoMapper() {

			this.ToTable("tb_area_associado_grupo");

            this.HasKey(o => o.id);
            
            this.HasOptional(x => x.Organizacao).WithMany().HasForeignKey(x => x.idOrganizacao);

		    this.HasOptional(x => x.AreaAssociadoGrupoDefault).WithMany().HasForeignKey(x => x.idGrupoDefault);

		}
	}
}