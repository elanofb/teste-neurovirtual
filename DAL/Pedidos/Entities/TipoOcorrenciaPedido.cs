using System.Data.Entity.ModelConfiguration;
using DAL.Entities;

namespace DAL.Pedidos {
	/**
	*
	*/

	public class TipoOcorrenciaPedido : Geral {
	}

	/**
	*
	*/

	internal sealed class TipoOcorrenciaPedidoMapper : EntityTypeConfiguration<TipoOcorrenciaPedido> {

		public TipoOcorrenciaPedidoMapper() {
			this.ToTable("datatb_tipo_ocorrencia_pedido");
			this.HasKey(o => o.id);
		}
	}
}