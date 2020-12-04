using System;
using System.Collections.Generic;
using DAL.Financeiro;
using FluentValidation.Attributes;

namespace WEB.Areas.FinanceiroLancamentos.ViewModels{
    
    [Validator(typeof(ConciliacaoAcaoFormValidator))]
	public class ConciliacaoAcaoForm{

		//Atributos
		
		//Propriedades
	    public List<ReceitaDespesaVW> listaLancamentos { get; set; }

	    public List<int> idsLancamentos { get; set; }
	    public List<string> tiposLancamentos { get; set; }
	    public DateTime? dtConciliacao { get; set; }
	    public bool flagAgrupar { get; set; }
        public string descricao { get; set; }

	    //Construtor
		public ConciliacaoAcaoForm(){

		}
    }

}