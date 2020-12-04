using BLL.Localizacao;
using FluentValidation;
using System;
using System.Linq;

namespace WEB.Areas.Localizacao.ViewModels {

    //
    public class EstadoValidator : AbstractValidator<EstadoVM> {
		
		//
		public EstadoValidator() {
            
            RuleFor(x => x.Estado.sigla)
				.NotEmpty().WithMessage("Informe qual é a sigla do estado.")
				.Must((x, sigla) => !exists(x)).WithMessage("Já existe um estado com a sigla informada.");

			RuleFor(x => x.Estado.nome)
				.NotEmpty().WithMessage("Informe qual é o nome do estado.");

			RuleFor(x => x.Estado.idPais)
				.NotEmpty().WithMessage("Informe o país no qual o estado está localizado.");

		}
        
        //
		private bool exists(EstadoVM ViewModel) {

			IEstadoBL OCidadeBL = new EstadoBL();

			int currentId = UtilNumber.toInt32(ViewModel.Estado.id);

            return OCidadeBL.listar("", "").Any(x => x.sigla == ViewModel.Estado.sigla && 
                                                     x.idPais == ViewModel.Estado.idPais && x.id != currentId);


		}

	}
}