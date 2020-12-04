using BLL.Localizacao;
using FluentValidation;
using System;
using System.Linq;

namespace WEB.Areas.Localizacao.ViewModels {

    //
    public class CidadeValidator : AbstractValidator<CidadeVM> {
		
		//
		public CidadeValidator() {
            
            RuleFor(x => x.Cidade.idEstado)
				.NotEmpty().WithMessage("Informe qual é o estado.");

			RuleFor(x => x.Cidade.nome)
				.NotEmpty().WithMessage("Informe qual é o nome da cidade.")
				.Must((x, nome) => !exists(x, nome)).WithMessage("Já existe uma cidade com esse nome no estado informado.");

			RuleFor(x => x.Cidade.flagCapital)
				.NotEmpty().WithMessage("Informe se a cidade é uma capital de estado ou não.");

		}
        
        //
		private bool exists(CidadeVM ViewModel, string nome) {

			ICidadeBL OCidadeBL = new CidadeBL();

			int currentId = UtilNumber.toInt32(ViewModel.Cidade.id);
			int idEstado = UtilNumber.toInt32(ViewModel.Cidade.idEstado);

            return OCidadeBL.listar(idEstado, "", "S").Any(x => x.nome == nome && x.id != currentId);

		}

	}
}