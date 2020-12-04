using FluentValidation;
using BLL.Associados;

namespace WEB.Areas.Associados.ViewModels {

    //
    public class MotivoDesativacaoFormValidator : AbstractValidator<MotivoDesativacaoForm> {

        //Atributos
        private MotivoDesativacaoBL _MotivoDesativacaoBL;

        //Propriedades
        private MotivoDesativacaoBL OMotivoDesativacaoBL => _MotivoDesativacaoBL = _MotivoDesativacaoBL ?? new MotivoDesativacaoBL();

        //
        public MotivoDesativacaoFormValidator() {

            RuleFor(x => x.MotivoDesativacao.descricao)
                .NotEmpty().WithMessage("Esse campo é obrigatório.")
                .Must( (x, descricao) => !this.existe(x))
                .WithMessage("Já existe um registro cadastrado com essa descrição.");
        }

        //
        private bool existe(MotivoDesativacaoForm ViewModel) {

            return OMotivoDesativacaoBL.existe(ViewModel.MotivoDesativacao, ViewModel.MotivoDesativacao.id);

        }
    }
}