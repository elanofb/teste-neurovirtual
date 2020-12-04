using System;
using System.Data.Entity.ModelConfiguration;
using DAL.Permissao;

namespace DAL.Pedidos {

	//
	public class PedidoHistorico {

		public int id { get; set; }

		public int idPedido { get; set; }

		public virtual Pedido Pedido { get; set; }

		public int idOcorrenciaPedido { get; set; }

		public virtual TipoOcorrenciaPedido TipoOcorrenciaPedido { get; set; }

		public DateTime dtOcorrencia { get; set; }

		public string observacoes { get; set; }

		public int? idUsuarioOcorrencia { get; set; }

		public virtual UsuarioSistema UsuarioOcorrencia { get; set; }

		public DateTime? dtEnvioCliente { get; set; }

		public DateTime dtCadastro { get; set; }

		public DateTime dtAlteracao { get; set; }

		public string ativo { get; set; }

		public string flagExcluido { get; set; }
	}

	//
	internal sealed class PedidoHistoricoMapper : EntityTypeConfiguration<PedidoHistorico> {

		public PedidoHistoricoMapper() {
			this.ToTable("tb_pedido_historico");
			this.HasKey(o => o.id);

			this.HasRequired(o => o.Pedido).WithMany().HasForeignKey(o => o.idPedido);
			this.HasRequired(o => o.TipoOcorrenciaPedido).WithMany().HasForeignKey(o => o.idOcorrenciaPedido);
			this.HasOptional(o => o.UsuarioOcorrencia).WithMany().HasForeignKey(o => o.idUsuarioOcorrencia);
		}
	}
}