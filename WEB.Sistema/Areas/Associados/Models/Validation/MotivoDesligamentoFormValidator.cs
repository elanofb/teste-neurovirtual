using FluentValidation;
using BLL.Associados;

namespace WEB.Areas.Associados.ViewModels {

    //
    public class MotivoDesligamentoFormValidator : AbstractValidator<MotivoDesligamentoForm> {

        //Atributos
        private MotivoDesligamentoBL _MotivoDesligamentoBL;

        //Propriedades
        private MotivoDesligamentoBL OMotivoDesligamentoBL => _MotivoDesligamentoBL = _MotivoDesligamentoBL ?? new MotivoDesligamentoBL();

        //
        public MotivoDesligamentoFormValidator() {

            RuleFor(x => x.MotivoDesligamento.descricao)
                .NotEmpty().WithMessage("Esse campo é obrigatório.")
                .Must( (x, descricao) => !this.existe(x))
                .WithMessage("Já existe um registro cadastrado com essa descrição.");
        }

        //
        private bool existe(MotivoDesligamentoForm ViewModel) {

            return OMotivoDesligamentoBL.existe(ViewModel.MotivoDesligamento, ViewModel.MotivoDesligamento.id);

        }
    }
}