using FluentValidation;
using FluentValidation.Attributes;
using DAL.Pessoas;

namespace WEB.Areas.Enderecos.ViewModels{

    [Validator(typeof(EnderecoVMValidator))]
	public class EnderecoVM{

		public int id { get; set;}
		public int idTipoEndereco { get; set;}
		public string cep { get; set;}
		public string logradouro { get; set;}
		public string numero { get; set;}
		public string complemento { get; set;}
		public string bairro { get; set;}
		public int? idCidade { get; set;}
		public string nomeCidade { get; set;}
		public int? idEstado { get; set;}
		public string nomeEstado { get; set;}
		public string idPais { get; set;}

		//
		public EnderecoVM() { 
			this.idPais = "BRA";
		}
	}

	//
	internal class EnderecoVMValidator : AbstractValidator<EnderecoVM> {
		
		//
        public EnderecoVMValidator(){

			RuleFor(x => x.cep).NotEmpty().WithMessage("Informe o CEP.").Length(8, 9).WithMessage("Informe um CEP válido.");
			RuleFor(x => x.logradouro).NotEmpty().WithMessage("Informe o endereço.");
			RuleFor(x => x.idEstado).NotEmpty().WithMessage("Informe o estado.");
			RuleFor(x => x.idCidade).NotEmpty().WithMessage("Informe a cidade.");

			RuleFor(x => x.idTipoEndereco)
				.NotEmpty().WithMessage("O campo 'Tipo de Endereço' é obrigatório.");

			RuleFor(x => x.logradouro)
				.NotEmpty().WithMessage("O campo 'logradouro' é obrigatório.");

			RuleFor(x => x.numero)
				.NotEmpty().WithMessage("O campo 'número' é obrigatório.");

			RuleFor(x => x.bairro)
				.NotEmpty().WithMessage("O campo 'bairro' é obrigatório.");

		 }
	}

	/**
	 * Validação da entidade Pessoa Endereço
	 */ 
	public class EnderecoValidator : AbstractValidator<PessoaEndereco> {
		
        public EnderecoValidator(){

			RuleFor(x => x.cep).NotEmpty().WithMessage("Informe o CEP.").Length(8, 9).WithMessage("Informe um CEP válido.");
			RuleFor(x => x.logradouro).NotEmpty().WithMessage("Informe o endereço.");
			RuleFor(x => x.idEstado).NotEmpty().WithMessage("Informe o estado.");
			RuleFor(x => x.idCidade).NotEmpty().WithMessage("Informe a cidade.");

			RuleFor(x => x.idTipoEndereco)
				.NotEmpty().WithMessage("O campo 'Tipo de Endereço' é obrigatório.");

			RuleFor(x => x.logradouro)
				.NotEmpty().WithMessage("O campo 'logradouro' é obrigatório.");

			RuleFor(x => x.numero)
				.NotEmpty().WithMessage("O campo 'número' é obrigatório.");

			RuleFor(x => x.bairro)
				.NotEmpty().WithMessage("O campo 'bairro' é obrigatório.");
		 }
	}
}