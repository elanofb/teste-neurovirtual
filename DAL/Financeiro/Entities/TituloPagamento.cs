using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Financeiro {
    public class TituloPagamento : Geral {

        public int? idFormaPagamento { get; set; }

		public virtual FormaPagamento FormaPagamento { get; set; }

        public decimal valorOriginal { get; set; }

		public decimal? valorPago { get; set; }

		public decimal? valorTarifaBancaria { get; set; }

		public decimal? valorJuros { get; set; }

		public DateTime dtVencimento { get; set; }

		public DateTime? dtPagamento { get; set; }

		public string nroBanco { get; set; }

		public string nroAgencia { get; set; }

		public int? nroConta { get; set; }

		public int? nroDigitoConta { get; set; }

		public string nroCarteira { get; set; }

		public string nroDocumento { get; set; }

		public string nossoNumero { get; set; }

		public string dacNossoNumero { get; set; }

		public DateTime? dtRealizacaoBaixa { get; set; }

		public DateTime? dtCredito { get; set; }

		public int? idUsuarioBaixa { get; set; }

		public DateTime? dtExclusao { get; set; }

		public string motivoExclusao { get; set; }

    }
}
