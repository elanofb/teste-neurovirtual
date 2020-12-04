using FluentValidation.Attributes;

namespace WEB.Areas.Financeiro.ViewModels{

    [Validator(typeof(TituloImpostoValidator))]
	public class TituloImpostoForm{
	    
        public TituloImposto TituloImposto { get; set; } 
        public int idTabelaImposto { get; set; }

	    public TituloImpostoForm() {
		    this.TituloImposto = new TituloImposto();
	    }

	}
}