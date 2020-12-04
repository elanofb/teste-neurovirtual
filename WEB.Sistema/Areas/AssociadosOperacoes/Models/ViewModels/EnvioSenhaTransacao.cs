using System.Collections.Generic;
using DAL.Associados.DTO;
using FluentValidation.Attributes;

namespace WEB.Areas.AssociadosOperacoes.ViewModels{
    
    [Validator(typeof(AssociadoEnvioSenhaTransacaoValidator))]
	public class EnvioSenhaTransacao {

		//Atributos
		
		//Propriedades
        public List<ItemListaAssociado> listaAssociados { get; set; }

		public List<int> idsAssociados { get; set; }
        
        public string novaSenha { get; set; }

	    //Construtor
		public EnvioSenhaTransacao(){

		}
        
	}

}