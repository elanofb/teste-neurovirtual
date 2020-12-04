using System;
using System.Data.Entity.ModelConfiguration;

namespace DAL.Origens {

	//
	public class OrigemOperacao {

		public byte id { get; set; }	
		
		public string descricao { get; set; }

		public bool? ativo { get; set; }

		public DateTime dtCadastro { get; set; }

	}

	//
	internal sealed class TipoTransacaoMapper : EntityTypeConfiguration<OrigemOperacao> {

		public TipoTransacaoMapper() {
			
			this.ToTable("datatb_origem_operacao");
			
			this.HasKey(o => o.id);
		}
	}
}