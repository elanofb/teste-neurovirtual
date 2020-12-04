using System.Collections.Generic;
using DAL.Associados.DTO;
using FluentValidation.Attributes;

namespace WEB.Areas.AssociadosOperacoes.ViewModels{
    
    [Validator(typeof(AssociadoDesativacaoFormValidator))]
	public class AssociadoDesativacaoForm {

		//Atributos
		
		//Propriedades
        public List<ItemListaAssociado> listaAssociados { get; set; }

		public List<int> idsAssociados { get; set; }

        public int? idMotivoDesativacao { get; set; }

        public string motivoDesativacao { get; set; }

	    //Construtor
		public AssociadoDesativacaoForm(){

		}
        
	}

}