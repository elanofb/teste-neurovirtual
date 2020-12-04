using FluentValidation.Attributes;
using DAL.AssociadosContribuicoes;

namespace WEB.Areas.AssociadosContribuicoes.ViewModels{

	[Validator(typeof(AssociadoContribuicaoFormValidator))]
	public class AssociadoContribuicaoPartialForm{

		//Atributos

		//Propriedades
		public AssociadoContribuicao AssociadoContribuicao{ get; set;}
        
	    //Construtor
		public AssociadoContribuicaoPartialForm(){
            
		}



	}

}