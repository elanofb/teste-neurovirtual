using System;
using System.Linq;
using FluentValidation;
using DAL.Contribuicoes;

namespace WEB.Areas.Contribuicoes.ViewModels{

    //
    public class ContribuicaoVencimentoValidator : AbstractValidator<ContribuicaoVencimento> {
        
		//Atributos

		//Propriedades

        //Construtor
        public ContribuicaoVencimentoValidator() {

		    this.validarVencimento();	

        }

        //
        private void validarVencimento() {

            RuleFor(x => x.diaVencimento)
                .NotEmpty().WithMessage("Informe o dia do vencimento.")
                .Must( (x, diaVencimento) => validarData(x.diaVencimento, x.mesVencimento)).WithMessage("Essa combinação dia/mês não pode ser usada.");

            RuleFor(x => x.mesVencimento)
                .NotEmpty().WithMessage("Informe o mês.");

            RuleFor(x => x.diaInicioVigencia)
                .NotEmpty().WithMessage("Informe o dia.")
                .Must( (x, diaInicioVigencia) => validarData(x.diaInicioVigencia, x.mesInicioVigencia)).WithMessage("Essa combinação dia/mês não pode ser usada.");

            RuleFor(x => x.mesInicioVigencia)
                .NotEmpty().WithMessage("Informe o mês.");

            RuleFor(x => x.diaFimVigencia)
                .NotEmpty().WithMessage("Informe o dia.")
                .Must( (x, diaFimVigencia) => validarData(x.diaFimVigencia, x.mesFimVigencia)).WithMessage("Essa combinação dia/mês não pode ser usada.");

            RuleFor(x => x.mesFimVigencia)
                .NotEmpty().WithMessage("Informe o mês.");

        }

        //
        private bool validarData(byte? dia, byte? mes) {

            if (dia > 28 && mes == 2) {
                return false;
            }

            int[] meses30Dias = new[] { 4, 6, 9, 11 };

            if (dia > 30 && meses30Dias.Contains(UtilNumber.toInt32(mes))) {
                return false;
            }


            return true;
        }
    }
}
