using System.Collections.Generic;
using DAL.Associados.DTO;
using FluentValidation.Attributes;

namespace WEB.Areas.AssociadosOperacoes.ViewModels{
    
    [Validator(typeof(AssociadoTipoAlteracaoFormValidator))]
	public class AssociadoTipoAlteracaoForm {

		//Atributos
		
		//Propriedades
        public List<ItemListaAssociado> listaAssociados { get; set; }

		public List<int> idsAssociados { get; set; }

        public int idTipoAssociado { get; set; }

        public string motivoAlteracao { get; set; }

	    //Construtor
		public AssociadoTipoAlteracaoForm(){

		}
        
	}

}