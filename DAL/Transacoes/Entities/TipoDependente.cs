using System;
using System.Data.Entity.ModelConfiguration;

namespace DAL.Transacoes {

	//
	public class TipoTransacao {

		public byte id { get; set; }	
		
		public string descricao { get; set; }

		public bool? ativo { get; set; }

		public DateTime dtCadastro { get; set; }

	}

	//
	internal sealed class TipoTransacaoMapper : EntityTypeConfiguration<TipoTransacao> {

		public TipoTransacaoMapper() {
			
			this.ToTable("datatb_tipo_transacao");
			
			this.HasKey(o => o.id);
		}
	}
}