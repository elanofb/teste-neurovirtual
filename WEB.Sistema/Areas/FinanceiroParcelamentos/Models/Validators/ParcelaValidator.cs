using System;
using DAL.Financeiro;
using FluentValidation;

namespace WEB.Areas.FinanceiroParcelamentos.ViewModels {

    //
    public class ParcelaValidator : AbstractValidator<TituloReceitaPagamento> {

        //Atributos

        //Propriedades

        //Construtor
        public ParcelaValidator() {

            RuleFor(x => x.dtVencimento)
                .NotEmpty().WithMessage("Informe uma data válida.")
                .GreaterThan(DateTime.Today.AddDays(-1)).WithMessage("Informe uma data válida.");

            //RuleFor(x => x.valorOriginal)
            //    .NotEmpty().WithMessage("O valor informado é inválido.")
            //    .GreaterThan(0).WithMessage("O valor informado é inválido");

        }
    }
}
