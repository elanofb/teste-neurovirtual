using System.Collections.Generic;
using DAL.Associados.DTO;
using FluentValidation.Attributes;

namespace WEB.Areas.AssociadosOperacoes.ViewModels{
    
    [Validator(typeof(AssociadoReativacaoFormValidator))]
	public class AssociadoReativacaoForm {

		//Atributos
		
		//Propriedades
        public List<ItemListaAssociado> listaAssociados { get; set; }

		public List<int> idsAssociados { get; set; }

        public string motivoReativacao { get; set; }

	    //Construtor
		public AssociadoReativacaoForm(){

		}
        
	}

}