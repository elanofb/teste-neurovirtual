using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using DAL.Organizacoes;
using DAL.Permissao;

namespace DAL.Notificacoes {

	public class NotificacaoSistema  {

        public int id { get; set; }

        public int idOrganizacao { get; set; }

        public int? idUnidade { get; set; }

        public virtual Organizacao Organizacao { get; set; }

        public byte? idTipoNotificacao { get; set; }

        public virtual TipoNotificacao TipoNotificacao { get; set; }

        public int? idReferencia { get; set; }

        public string titulo { get; set; }
		
		public string tituloPush { get; set; }
		
		public int? idTemplate { get; set; }

        public string notificacaoTexto { get; set; }
		
        public string notificacao { get; set; }
		
		public string notificacaoPush { get; set; }

        public DateTime? dtProgramacaoEnvio { get; set; }

		public bool? flagSistema { get; set; }

		public bool? flagEmail { get; set; }
		
		public DateTime? dtFinalizacaoEnvioEmail { get; set; }

		public bool? flagMobile { get; set; }
		
		public byte? idGatewayPush { get; set; }
		
		public DateTime? dtFinalizacaoEnvioPush { get; set; }
		
		public bool? flagTodosAssociados { get; set; }

		public bool? flagAssociadosInadimplentes { get; set; }

		public bool? flagAssociadosAdimplentes { get; set; }

        public string flagStatusAssociados { get; set; }

		public bool? flagAssociadosEspecificos { get; set; }

		public bool? flagTodosUsuarios { get; set; }

		public bool? flagUsuariosEspecificos { get; set; }

		public DateTime dtCadastro { get; set; }

        public int? idUsuarioCadastro { get; set; }

        public virtual UsuarioSistema UsuarioCadastro { get; set; }

        public DateTime? dtAlteracao { get; set; }

		public int? idUsuarioAlteracao { get; set; }

		public bool? ativo { get; set; }

		public bool? flagExcluido { get; set; }

        public IList<NotificacaoSistemaEnvio> listaPessoa { get; set; }

        /// <summary>
        /// Construtor
        /// </summary>
		public NotificacaoSistema() {

            this.listaPessoa = new List<NotificacaoSistemaEnvio>();

		}
	}

	//
	internal sealed class NotificacaoSistemaMapper : EntityTypeConfiguration<NotificacaoSistema> {

		public NotificacaoSistemaMapper() {

			this.ToTable("systb_notificacao_sistema");

            this.HasKey(o => o.id);

		    this.HasRequired(x => x.Organizacao).WithMany().HasForeignKey(x => x.idOrganizacao);

            this.HasOptional(x => x.TipoNotificacao).WithMany().HasForeignKey(x => x.idTipoNotificacao);

            this.HasOptional(x => x.UsuarioCadastro).WithMany().HasForeignKey(x => x.idUsuarioCadastro);

		}
	}
}