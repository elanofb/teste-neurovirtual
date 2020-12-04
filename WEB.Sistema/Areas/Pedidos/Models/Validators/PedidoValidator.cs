using System;
using DAL.Financeiro;
using FluentValidation;

namespace WEB.Areas.Pedidos.ViewModels{

	public class PedidoPagamentoValidator : AbstractValidator<PedidoPagamentoVM> {
		
		public PedidoPagamentoValidator() {

            RuleFor(x => x.listaPagamentos).SetCollectionValidator( new PedidoPagamentoParcelaValidator());
			
		 }


	}

	public class PedidoPagamentoParcelaValidator : AbstractValidator<TituloReceitaPagamento> {
		
		public PedidoPagamentoParcelaValidator() {

		    RuleFor(x => x.valorOriginal)
		        .GreaterThan(0).WithMessage("O valor da parcela deve ser maior do que zero.");

		    RuleFor(x => x.dtVencimento)
		        .GreaterThanOrEqualTo(DateTime.Today).WithMessage("A data para vencimento deve ser maior ou igual a data de hoje.");

		}


	}

}