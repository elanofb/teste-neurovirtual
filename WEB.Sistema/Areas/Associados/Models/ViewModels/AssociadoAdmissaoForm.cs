using System;
using System.Collections.Generic;
using DAL.Associados.DTO;
using FluentValidation.Attributes;

namespace WEB.Areas.Associados.ViewModels{
    
    [Validator(typeof(AssociadoAdmissaoFormValidator))]
	public class AssociadoAdmissaoForm{

		//Atributos
		
		//Propriedades
        public List<ItemListaAssociado> listaAssociados { get; set; }

		public List<int> idsAssociados { get; set; }

        public DateTime? dtAdmissao { get; set; }

        public string observacoes { get; set; }

	    //Construtor
		public AssociadoAdmissaoForm(){

		}
        
	}

}