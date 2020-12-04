using System;
using FluentValidation;
using DAL.Pedidos;

namespace WEB.Areas.Pedidos.ViewModels{

	public class PedidoEntregaValidator : AbstractValidator<PedidoEntrega> {
		
		//
        public PedidoEntregaValidator(){

			 RuleFor(x => x.idPais)
				 .NotEmpty().WithMessage("O campo 'País' é obrigatório.");

			 When(x => !String.IsNullOrEmpty(x.idPais) && x.idPais.Equals("BRA"), () => {
				 RuleFor(x => x.cep).NotEmpty().WithMessage("Informe o CEP.").Length(8, 9).WithMessage("Informe um CEP válido.");
				 RuleFor(x => x.logradouro).NotEmpty().WithMessage("Informe o endereço.");
				 RuleFor(x => x.idEstado).NotEmpty().WithMessage("Informe o estado.");
				 RuleFor(x => x.idCidade).NotEmpty().WithMessage("Informe a cidade.");
			 });


			 When(x => !String.IsNullOrEmpty(x.idPais) && !x.idPais.Equals("BRA"), () => {
				 RuleFor(x => x.idPais).NotEmpty().WithMessage("Informe qual é o país.");
				 RuleFor(x => x.nomeCidade).NotEmpty().WithMessage("Informe a cidade.");
			 });

			 RuleFor(x => x.logradouro)
				 .NotEmpty().WithMessage("O campo 'logradouro' é obrigatório.");

			 RuleFor(x => x.numero)
				 .NotEmpty().WithMessage("O campo 'número' é obrigatório.");

			 RuleFor(x => x.bairro)
				 .NotEmpty().WithMessage("O campo 'bairro' é obrigatório.");

		 }
	}
}