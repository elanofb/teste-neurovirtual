using System;
using System.Collections.Generic;
using System.Linq;
using DAL.Financeiro;

namespace WEB.Areas.Financeiro.ViewModels{

	public class ModalDetalhePagamento{
        
		public TituloReceita TituloReceita { get; set; }

		public IList<TituloReceitaPagamento> listaPagamentos { get; set; }

		public IList<TituloReceitaPagamento> listaPagamentosCancelados { get; set; }

        public decimal valorParcelado { get; set; }

        public decimal valorDiferencaParcelas { get; set; }

        public TituloReceitaPagamento ParcelaAdicional { get; set;}

		//Construtor
		public ModalDetalhePagamento() {
			
			this.listaPagamentos = new List<TituloReceitaPagamento>();

			this.listaPagamentosCancelados = new List<TituloReceitaPagamento>();

            this.ParcelaAdicional = new TituloReceitaPagamento();
		}

        //
	    public void carregarDados(TituloReceita OTitulo) {

		    this.TituloReceita = OTitulo;

            this.listaPagamentos = OTitulo.listaTituloReceitaPagamento.Where(x => x.dtExclusao == null).OrderBy(x => x.dtVencimento).ToList();

            this.listaPagamentosCancelados = OTitulo.listaTituloReceitaPagamento.Where(x => x.dtExclusao != null).OrderByDescending(x => x.id).ToList();
	        
		    this.valorParcelado = this.listaPagamentos.Sum(x => x.valorOriginal);

		    this.valorDiferencaParcelas = Decimal.Subtract(UtilNumber.toDecimal(OTitulo.valorTotal), this.valorParcelado);

	    }
	}
}