using System;
using System.Data.Entity.ModelConfiguration;
using DAL.Entities;
using DAL.Notificacoes;
using DAL.Permissao;

namespace DAL.Associados {
	/**
	*
	*/

	public class AssociadoNotificacao : DefaultEntity {

		public int? idNotificacao { get; set; }

		public virtual NotificacaoSistema OAvisoNotificacao { get; set; }

		public int? idAssociado { get; set; }

		public virtual Associado OAssociado { get; set; }

		public int? idUsuario { get; set; }

		public virtual UsuarioSistema OUsuarioSistema { get; set; }

		public DateTime? dtLeitura { get; set; }
	}

	/**
	*
	*/

	public class AssociadoNotificacaoMapper : EntityTypeConfiguration<AssociadoNotificacao> {

		public AssociadoNotificacaoMapper() {
			this.ToTable("tb_associado_notificacao");
			this.HasKey(x => x.id);
			this.HasOptional(x => x.OAssociado).WithMany().HasForeignKey(x => x.idAssociado);
			this.HasOptional(x => x.OUsuarioSistema).WithMany().HasForeignKey(x => x.idUsuario);
		}
	}
}