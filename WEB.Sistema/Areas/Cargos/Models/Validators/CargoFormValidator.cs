using FluentValidation;
using BLL.Cargos;

namespace WEB.Areas.Cargos.ViewModels {

    //
    public class CargoFormValidator : AbstractValidator<CargoForm> {

        //Atributos
        private CargoBL _CargoBL;

        //Propriedades
        private CargoBL OCargoBL => _CargoBL = _CargoBL ?? new CargoBL();

        //
        public CargoFormValidator() {

            RuleFor(x => x.Cargo.descricao)
                .NotEmpty().WithMessage("Esse campo é obrigatório.")
                .Must( (x, descricao) => !this.existe(x))
                .WithMessage("Já existe um registro cadastrado com essa descrição.");
        }

        //
        private bool existe(CargoForm ViewModel) {

            return OCargoBL.existe(ViewModel.Cargo, ViewModel.Cargo.id);

        }
    }
}