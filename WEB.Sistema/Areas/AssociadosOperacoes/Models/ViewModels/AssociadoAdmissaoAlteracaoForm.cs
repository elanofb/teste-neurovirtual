using System;
using System.Collections.Generic;
using DAL.Associados.DTO;
using FluentValidation.Attributes;

namespace WEB.Areas.AssociadosOperacoes.ViewModels{
    
    [Validator(typeof(AssociadoAdmissaoAlteracaoFormValidator))]
	public class AssociadoAdmissaoAlteracaoForm {

		//Atributos
		
		//Propriedades
        public List<ItemListaAssociado> listaAssociados { get; set; }

		public List<int> idsAssociados { get; set; }

        public DateTime? dtAdmissao { get; set; }

        public string motivoAlteracao { get; set; }

	    //Construtor
		public AssociadoAdmissaoAlteracaoForm(){

		}
        
	}

}