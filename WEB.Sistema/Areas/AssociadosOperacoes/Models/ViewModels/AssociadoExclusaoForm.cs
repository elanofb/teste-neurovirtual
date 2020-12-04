using System.Collections.Generic;
using DAL.Associados.DTO;
using FluentValidation.Attributes;

namespace WEB.Areas.AssociadosOperacoes.ViewModels{
    
    [Validator(typeof(AssociadoExclusaoFormValidator))]
	public class AssociadoExclusaoForm{

		//Atributos
		
		//Propriedades
        public List<ItemListaAssociado> listaAssociados { get; set; }

		public List<int> idsAssociados { get; set; }

        public int? idMotivoDesligamento { get; set; }

        public string motivoExclusao { get; set; }

	    //Construtor
		public AssociadoExclusaoForm(){

		}
        
	}

}