using DAL.Pessoas;
using FluentValidation.Attributes;

namespace WEB.Areas.Pessoas.ViewModels {

	[Validator(typeof(PessoaContatoFormValidator))]
	public class PessoaContatoForm {

		//Propriedades
		public PessoaContato PessoaContato { get; set; }

		//Construtor
		public PessoaContatoForm() { 
		}

	}
}