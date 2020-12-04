using System.Data.Entity.ModelConfiguration;
using DAL.Organizacoes;

namespace DAL.ConfiguracoesAreaAssociado {

	//
	public class AreaAssociadoGrupoDefault {

		public int id { get; set; }
        
        public string descricao { get; set; }

		public string area { get; set; }

		public string controller { get; set; }

		public string action { get; set; }

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
	internal sealed class AreaAssociadoGrupoDefaultMapper : EntityTypeConfiguration<AreaAssociadoGrupoDefault> {

		public AreaAssociadoGrupoDefaultMapper() {

			this.ToTable("datatb_area_associado_grupo");

            this.HasKey(o => o.id);
            
		}
	}
}