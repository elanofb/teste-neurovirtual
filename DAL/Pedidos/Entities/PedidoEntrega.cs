using System;
using System.Data.Entity.ModelConfiguration;
using DAL.Enderecos;
using DAL.Localizacao;
using DAL.Frete;

namespace DAL.Pedidos {

	//
	public class PedidoEntrega {

		public int id { get; set; }

		public int idPedido { get; set; }

		public virtual Pedido Pedido { get; set; }

		public string idPais { get; set; }

		public virtual Pais Pais { get; set; }

		public int? idEstado { get; set; }

		public virtual Estado Estado { get; set; }

		public int? idCidade { get; set; }

		public virtual Cidade Cidade { get; set; }

		public byte? idTipoEndereco { get; set; }

		public virtual TipoEndereco TipoEndereco { get; set; }

		public string nomeCidade { get; set; }

	    public string cepOrigem { get; set; }

		public string cep { get; set; }

		public string logradouro { get; set; }

		public string numero { get; set; }

		public string complemento { get; set; }

		public string bairro { get; set; }

        public DateTime? dtAgendamentoEntrega { get; set; }

        public string flagPeriodo { get; set; }

        public string nroRastreamentoEntrega { get; set; }

		public int? idTipoFrete { get; set; }

        public virtual TipoFrete TipoFrete { get; set; }

		public int? idTransportador { get; set; }

		public virtual Transportador Transportador { get; set; }

		public DateTime dtCadastro { get; set; }

		public int idUsuarioCadastro { get; set; }

		public DateTime dtAlteracao { get; set; }

		public int idUsuarioAlteracao { get; set; }

		public string flagExcluido { get; set; }

	}

	//
	internal sealed class PedidoEntregaMapper : EntityTypeConfiguration<PedidoEntrega> {

		public PedidoEntregaMapper() {
			this.ToTable("tb_pedido_entrega");
			this.HasKey(o => o.id);

			this.HasRequired(o => o.Pedido).WithMany(x => x.listaPedidoEntrega).HasForeignKey(x => x.idPedido);

            this.HasRequired(o => o.Pais).WithMany().HasForeignKey(o => o.idPais);

            this.HasOptional(o => o.Estado).WithMany().HasForeignKey(o => o.idEstado);

            this.HasOptional(o => o.Cidade).WithMany().HasForeignKey(o => o.idCidade);

            this.HasOptional(o => o.TipoEndereco).WithMany().HasForeignKey(o => o.idTipoEndereco);

            this.HasOptional(o => o.TipoFrete).WithMany().HasForeignKey(o => o.idTipoFrete);

            this.HasOptional(o => o.Transportador).WithMany().HasForeignKey(o => o.idTransportador);
		}
	}
}