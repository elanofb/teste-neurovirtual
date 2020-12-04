using FluentValidation.Attributes;

namespace WEB.Areas.NaoAssociados.ViewModels{

    [Validator(typeof(NaoAssociadoEnvioCadastroEmailFormValidator))]
	public class NaoAssociadoEnvioCadastroEmailForm {

		//Atributos

		//Propriedades
        public int idAssociado { get; set;}
		public string emailsDestino { get; set;}
		
		//Servicos

		//Construtor
		public NaoAssociadoEnvioCadastroEmailForm() {
            
		}

	}

}