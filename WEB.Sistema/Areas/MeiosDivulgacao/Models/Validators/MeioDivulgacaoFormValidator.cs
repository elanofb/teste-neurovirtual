using BLL.MeiosDivulgacao;
using FluentValidation;

namespace WEB.Areas.MeiosDivulgacao.ViewModels {

    //
    public class MeioDivulgacaoFormValidator : AbstractValidator<MeioDivulgacaoForm> {

        //Atributos
        private IMeioDivulgacaoBL _MeioDivulgacaoBL;

        //Propriedades
        private IMeioDivulgacaoBL OMeioDivulgacaoBL => _MeioDivulgacaoBL = _MeioDivulgacaoBL ?? new MeioDivulgacaoBL();

        //
        public MeioDivulgacaoFormValidator() {

            RuleFor(x => x.MeioDivulgacao.descricao)
                .NotEmpty().WithMessage("Esse campo é obrigatório.")
                .Must( (x, descricao) => !this.existe(x))
                .WithMessage("Já existe um registro cadastrado com essa descrição.");
        }

        //
        private bool existe(MeioDivulgacaoForm ViewModel) {

            return OMeioDivulgacaoBL.existe(ViewModel.MeioDivulgacao, ViewModel.MeioDivulgacao.id);

        }
    }
}