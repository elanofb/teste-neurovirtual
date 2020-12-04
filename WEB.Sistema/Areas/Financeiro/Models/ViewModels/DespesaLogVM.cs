using System.Collections.Generic;
using DAL.Financeiro;
using DAL.LogsAlteracoes;

namespace WEB.Areas.Financeiro.ViewModels{

	public class DespesaLogVM {
		
        public TituloDespesa TituloDespesa { get; set; }

        public TituloDespesaPagamento TituloDespesaPagamento { get; set; }

        public List<LogAlteracao> listaLogAlteracao { get; set; }

	    public DespesaLogVM(){
	        this.listaLogAlteracao = new List<LogAlteracao>();
        }
    }
}