using FluentValidation;
using BLL.RamosAtividade;

namespace WEB.Areas.RamosAtividade.ViewModels {

    //
    public class RamoAtividadeFormValidator : AbstractValidator<RamoAtividadeForm> {

        //Atributos
        private IRamoAtividadeBL _RamoAtividadeBL;

        //Propriedades
        private IRamoAtividadeBL ORamoAtividadeBL => _RamoAtividadeBL = _RamoAtividadeBL ?? new RamoAtividadeBL();

        //
        public RamoAtividadeFormValidator() {

            RuleFor(x => x.RamoAtividade.descricao)
                .NotEmpty().WithMessage("Esse campo é obrigatório.")
                .Must( (x, descricao) => !this.existe(x))
                .WithMessage("Já existe um registro cadastrado com essa descrição.");
        }

        //
        private bool existe(RamoAtividadeForm ViewModel) {

            return ORamoAtividadeBL.existe(ViewModel.RamoAtividade, ViewModel.RamoAtividade.id);

        }
    }
}