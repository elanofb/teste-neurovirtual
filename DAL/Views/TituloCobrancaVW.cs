using System;
using System.Data.Entity.ModelConfiguration;

namespace DAL.Entities {

	[Serializable]
	public class TituloCobrancaVW {

		public int idTituloCobranca { get; set; }

		public int idTituloCobrancaPagamento { get; set; }

		public int idTipoCobranca { get; set; }

		public string nomeTipoCobranca { get; set; }

		public int idCobranca { get; set; }

		public int idPessoa { get; set; }

		public string nomePessoa { get; set; }

		public string documentoPessoa { get; set; }

		public string nomeRecibo { get; set; }

		public string documentoRecibo { get; set; }

		public decimal valorTotal { get; set; }

		public decimal valorJurosTitulo { get; set; }

		public decimal valorDesconto { get; set; }

		public DateTime? dtQuitacaoTotal { get; set; }

		public DateTime dtCadastroTitulo { get; set; }

		public int idFormaPagamento { get; set; }

		public string descricaoFormaPagamento { get; set; }

		public decimal valorOriginalParcela { get; set; }

		public decimal valorPago { get; set; }

		public decimal valorTarifasBancarias { get; set; }

		public decimal valorJuros { get; set; }

		public DateTime dtVencimentoParcela { get; set; }

		public DateTime? dtPagamentoParcela { get; set; }

		public DateTime? dtCreditoParcela { get; set; }

		public string nroDocumento { get; set; }

		public string nossoNumero { get; set; }

		public string dacNossoNumero { get; set; }

		public string nroCarteira { get; set; }

		public int idCobrancaServico { get; set; }

		public string descricaoServico { get; set; }

		public string nroBanco { get; set; }

		public string nroAgencia { get; set; }

		public string nroConta { get; set; }
	}

	//
	internal sealed class TituloCobrancaVWMapper : EntityTypeConfiguration<TituloCobrancaVW> {

		public TituloCobrancaVWMapper() {
			this.ToTable("vw_titulo_cobranca");
			this.HasKey(o => o.idTituloCobrancaPagamento);
		}
	}
}