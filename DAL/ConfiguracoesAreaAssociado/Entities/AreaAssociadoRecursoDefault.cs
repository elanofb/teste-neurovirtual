using System;
using System.Data.Entity.ModelConfiguration;
using DAL.Organizacoes;
using DAL.Permissao;

namespace DAL.ConfiguracoesAreaAssociado {

	//
	public class AreaAssociadoRecursoDefault {

		public int id { get; set; }
        
        public int? idRecursoPai { get; set; }

		public virtual AreaAssociadoRecursoDefault RecursoPaiDefault { get; set; }

		public int? idRecursoGrupo { get; set; }

		public virtual AreaAssociadoGrupoDefault RecursoGrupoDefault { get; set; }

		public string nomeDisplay { get; set; }

		public string area { get; set; }

		public string controller { get; set; }

		public string actionPadrao { get; set; }

        public string descricao { get; set; }

	    public int ordemExibicao { get; set; }

	    public bool? flagAcessoLiberado { get; set; }

	    public bool? flagMenu { get; set; }

	    public bool? flagPF { get; set; }

	    public bool? flagPJ { get; set; }

	    public bool? flagAssociado { get; set; }

	    public bool? flagNaoAssociado { get; set; }

	    public bool? flagEmAdmissao { get; set; }

	    public bool? flagAdimplente { get; set; }

	    public bool? flagInadimplente { get; set; }
        
        public DateTime? dtCadastro { get; set; }

		public DateTime? dtAlteracao { get; set; }

		public int idUsuarioCadastro { get; set; }

		public int idUsuarioAlteracao { get; set; }

        public bool? ativo { get; set; }

		public bool? flagExcluido { get; set; }

	}

	//
	internal sealed class AreaAssociadoRecursoDefaultMapper : EntityTypeConfiguration<AreaAssociadoRecursoDefault> {

		public AreaAssociadoRecursoDefaultMapper() {

			this.ToTable("datatb_area_associado_recurso");

            this.HasKey(o => o.id);
            
            this.HasOptional(x => x.RecursoPaiDefault).WithMany().HasForeignKey(x => x.idRecursoPai);

            this.HasOptional(x => x.RecursoGrupoDefault).WithMany().HasForeignKey(x => x.idRecursoGrupo);

		}
	}
}