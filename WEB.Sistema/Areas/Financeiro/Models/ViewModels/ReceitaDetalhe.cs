using System.Collections.Generic;
using DAL.Financeiro;

namespace WEB.Areas.Financeiro.ViewModels{

	public class ReceitaDetalhe{
        
		public TituloReceita TituloReceita { get; set; }

		public IList<TituloReceitaPagamento> listaPagamentos { get; set; }

		public IList<TituloReceitaPagamento> listaPagamentosCancelados { get; set; }

        public decimal valorParcelado { get; set; }

        public decimal valorDiferencaParcelas { get; set; }

		//Construtor
		public ReceitaDetalhe() {
			
			this.listaPagamentos = new List<TituloReceitaPagamento>();

			this.listaPagamentosCancelados = new List<TituloReceitaPagamento>();
		}

	}
}