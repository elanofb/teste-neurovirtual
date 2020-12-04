using FluentValidation;
using BLL.RamosAtividade;

namespace WEB.Areas.RamosAtividade.ViewModels {

    //
    public class SetorAtuacaoFormValidator : AbstractValidator<SetorAtuacaoForm> {

        //Atributos
        private ISetorAtuacaoBL _SetorAtuacaoBL;

        //Propriedades
        private ISetorAtuacaoBL OSetorAtuacaoBL => _SetorAtuacaoBL = _SetorAtuacaoBL ?? new SetorAtuacaoBL();

        //
        public SetorAtuacaoFormValidator() {

            RuleFor(x => x.SetorAtuacao.idRamoAtividade)
                .NotEmpty().WithMessage("Informe o ramo de atividade.");

            RuleFor(x => x.SetorAtuacao.descricao)
                .NotEmpty().WithMessage("Esse campo é obrigatório.")
                .Must( (x, descricao) => !this.existe(x))
                .WithMessage("Já existe um registro cadastrado com essa descrição.");
        }

        //
        private bool existe(SetorAtuacaoForm ViewModel) {

            return OSetorAtuacaoBL.existe(ViewModel.SetorAtuacao, ViewModel.SetorAtuacao.id);

        }
    }
}