using System;
using System.Data.Entity.ModelConfiguration;
using DAL.Organizacoes;

namespace DAL.Notificacoes {

    public class NotificacaoSistemaEnvio {

		public int id { get; set; }

        public int idOrganizacao { get; set; }

        public Organizacao Organizacao {get; set; }  

        public int idNotificacao { get; set; }

        public NotificacaoSistema NotificacaoSistema {get; set; }  
        
        public int? idReferencia { get; set; }
	    
	    public int? idPessoa { get; set; }
        
        public string nome { get; set; }

	    public string email { get; set; }
		
		public int? nroMembro { get; set; }
	    
	    public string personalizacao { get; set; }

        public int? idTarefa { get; set; }

		public bool flagEnvioEmail { get; set; }

		public DateTime? dtEnvioEmail { get; set; }

        public DateTime? dtLeitura { get; set; }

		public DateTime? dtCadastro { get; set; }

		public int? idUsuarioCadastro { get; set; }

		public DateTime? dtAlteracao { get; set; }

		public int? idUsuarioAlteracao { get; set; }

		public bool? ativo { get; set; }

		public bool? flagExcluido { get; set; }

        public string motivoExclusao { get; set; }

    }


    public class NotificacaoSistemaEnvioMapper : EntityTypeConfiguration<NotificacaoSistemaEnvio> {

        public NotificacaoSistemaEnvioMapper() {

            this.ToTable("systb_notificacao_sistema_envio");

            this.HasKey(x => x.id);

            this.HasRequired(x => x.Organizacao).WithMany().HasForeignKey(x => x.idOrganizacao);

            this.HasRequired(x => x.NotificacaoSistema).WithMany(p => p.listaPessoa).HasForeignKey(x => x.idNotificacao);
        }
    }
}
