using System;
using System.Data.Entity.ModelConfiguration;

namespace DAL.RedeAfiliados {

	//
	public class RedePontuacao {

        public int id { get; set; }
		
		public int idMembro { get; set; }
		
		public int? nroMembro { get; set; }

		public decimal qtdePontos { get; set; }
		
		public bool? flagLadoEsquerdo { get; set; }
		
		public bool? flagLadoDireito { get; set; }
		
		public bool flagPago { get; set; }
		
		public int? idPedidoProdutoOrigem { get; set; }
		
		public int? idMembroOrigem { get; set; }

		public DateTime dtCadastro { get; set; }
		
		public DateTime? dtExclusao { get; set; }
		
		public bool flagImportado { get; set; }

	}

	//
	internal sealed class RedePontuacaoMapper : EntityTypeConfiguration<RedePontuacao> {

		public RedePontuacaoMapper() {

			this.ToTable("tb_rede_pontuacao");

			this.HasKey(o => o.id);


		}
	}
}

