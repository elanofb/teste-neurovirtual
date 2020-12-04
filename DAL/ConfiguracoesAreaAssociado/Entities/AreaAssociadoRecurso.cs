using System;
using System.Data.Entity.ModelConfiguration;
using DAL.Organizacoes;
using DAL.Permissao;

namespace DAL.ConfiguracoesAreaAssociado {

	//
	public class AreaAssociadoRecurso {

		public int id { get; set; }

        public int? idOrganizacao { get; set; }

        public virtual Organizacao Organizacao {get; set;}
        
	    public int? idRecursoDefault { get; set; }

	    public virtual AreaAssociadoRecursoDefault RecursoDefault { get; set; }

        public int? idRecursoPai { get; set; }

		public virtual AreaAssociadoRecurso RecursoPai { get; set; }

		public int? idRecursoGrupo { get; set; }

		public virtual AreaAssociadoGrupo RecursoGrupo { get; set; }

		public string nomeDisplay { get; set; }

		public string area { get; set; }

		public string controller { get; set; }

		public string actionPadrao { get; set; }

	    public string linkAbsoluto { get; set; }

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
	internal sealed class AreaAssociadoRecursoMapper : EntityTypeConfiguration<AreaAssociadoRecurso> {

		public AreaAssociadoRecursoMapper() {

			this.ToTable("tb_area_associado_recurso");

            this.HasKey(o => o.id);
            
            this.HasOptional(x => x.Organizacao).WithMany().HasForeignKey(x => x.idOrganizacao);

		    this.HasOptional(x => x.RecursoDefault).WithMany().HasForeignKey(x => x.idRecursoDefault);

            this.HasOptional(x => x.RecursoPai).WithMany().HasForeignKey(x => x.idRecursoPai);

            this.HasOptional(x => x.RecursoGrupo).WithMany().HasForeignKey(x => x.idRecursoGrupo);

		}
	}
}