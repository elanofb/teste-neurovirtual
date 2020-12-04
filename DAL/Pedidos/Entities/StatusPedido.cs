using System.Data.Entity.ModelConfiguration;

namespace DAL.Pedidos {

	public class StatusPedido {

		public int id { get; set; }

        public string descricao { get; set; }

        public string flagFinalizador { get; set; }

        public string ativo { get; set; }

        public string flagExcluido { get; set; }
	}

	internal sealed class StatusPedidoMapper : EntityTypeConfiguration<StatusPedido> {

		public StatusPedidoMapper() {
			this.ToTable("datatb_status_pedido");
			this.HasKey(o => o.id);
		}
	}
}