using System.Collections.Generic;
using DAL.Associados.DTO;
using DAL.Notificacoes;
using FluentValidation.Attributes;

namespace WEB.Areas.AssociadosNotificacoes.ViewModels{
    
    [Validator(typeof(AssociadoNotificacaoFormValidator))]
	public class AssociadoNotificacaoForm{

		//Atributos
		
		//Propriedades
        public List<ItemListaAssociado> listaAssociados { get; set; }

		public List<int> idsAssociados { get; set; }

        public NotificacaoSistema ONotificacao { get; set; }

	    //Construtor
		public AssociadoNotificacaoForm(){

		}
        
	}

}