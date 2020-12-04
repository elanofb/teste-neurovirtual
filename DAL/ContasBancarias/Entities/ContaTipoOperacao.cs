using System.Data.Entity.ModelConfiguration;
using DAL.Entities;

namespace DAL.ContasBancarias {

	public class ContaTipoOperacao : Geral {
	}

	internal sealed class ContaTipoOperacaoMapper : EntityTypeConfiguration<ContaTipoOperacao> {

		public ContaTipoOperacaoMapper() {
			this.ToTable("datatb_conta_tipo_operacao");
			this.HasKey(x => x.id);
		}
	}
}