using System;
using DAL.Entities;

namespace DAL.Notificacoes {

	public class AvisoNotificacao : DefaultEntity {

		public int? idNotificacao { get; set; }

		public int? idPessoa { get; set; }

		public DateTime? dtLeitura { get; set; }
	}
}