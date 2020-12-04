using System.Collections.Generic;
using DAL.Financeiro;
using DAL.LogsAlteracoes;

namespace WEB.Areas.Financeiro.ViewModels{

	public class ReceitaLogVM {
		
        public TituloReceita TituloReceita { get; set; }

        public TituloReceitaPagamento TituloReceitaPagamento { get; set; }

        public List<LogAlteracao> listaLogAlteracao { get; set; }

	    public ReceitaLogVM(){
	        this.listaLogAlteracao = new List<LogAlteracao>();
	    }
    }
}