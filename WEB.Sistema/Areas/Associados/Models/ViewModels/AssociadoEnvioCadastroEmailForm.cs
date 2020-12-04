using FluentValidation.Attributes;

namespace WEB.Areas.Associados.ViewModels{

    [Validator(typeof(AssociadoEnvioCadastroEmailFormValidator))]
	public class AssociadoEnvioCadastroEmailForm {

		//Atributos

		//Propriedades
        public int idAssociado { get; set;}
		public string emailsDestino { get; set;}
		
		//Servicos

		//Construtor
		public AssociadoEnvioCadastroEmailForm() {
            
		}

	}

}