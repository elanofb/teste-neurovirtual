using FluentValidation;
using BLL.AreasAtuacao;

namespace WEB.Areas.AreasAtuacao.ViewModels {

    //
    public class AreaAtuacaoFormValidator : AbstractValidator<AreaAtuacaoForm> {

        //Atributos
        private AreaAtuacaoBL _AreaAtuacaoBL;

        //Propriedades
        private AreaAtuacaoBL OAreaAtuacaoBL => _AreaAtuacaoBL = _AreaAtuacaoBL ?? new AreaAtuacaoBL();

        //
        public AreaAtuacaoFormValidator() {

            RuleFor(x => x.AreaAtuacao.descricao)
                .NotEmpty().WithMessage("Esse campo é obrigatório.")
                .Must( (x, descricao) => !this.existe(x))
                .WithMessage("Já existe um registro cadastrado com essa descrição.");
        }

        //
        private bool existe(AreaAtuacaoForm ViewModel) {

            return OAreaAtuacaoBL.existe(ViewModel.AreaAtuacao, ViewModel.AreaAtuacao.id);

        }
    }
}