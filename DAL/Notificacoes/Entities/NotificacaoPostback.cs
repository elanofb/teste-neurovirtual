using System;
using System.Data.Entity.ModelConfiguration;
using DAL.Entities;

namespace DAL.Notificacoes {

	public class NotificacaoPostback {

		public int id { get; set; }
		
		public byte? idGateway { get; set; }
		
		public string idExternoNotificacao { get; set; }
		
		public string acao { get; set; }
		
		public string contaOrigem { get; set; }
		
		public string contaDestino { get; set; }
		
		public DateTime? dtAcao { get; set; }
		
		public string ipAcao { get; set; }
		
		public string meioInteracao { get; set; }
		
		public bool? flagAtualizado { get; set; }
		
		public bool? ativo { get; set; }
		
		public DateTime dtCadastro { get; set; }
		
		public int idUsuarioCadastro { get; set; }
		
		public DateTime? dtExclusao { get; set; }
		
		public int? idUsuarioExclusao { get; set; }
	}

	//
	internal sealed class NotificacaoPostbackMapper : EntityTypeConfiguration<NotificacaoPostback> {

		public NotificacaoPostbackMapper() {

			this.ToTable("tb_notificacao_postback");

			this.HasKey(o => o.id);

		}
	}
}