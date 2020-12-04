using FluentValidation;

namespace WEB.Areas.Produtos.ViewModels{

    //
    public class ProdutoComposicaoValidator : AbstractValidator<ProdutoComposicaoForm> {

        //Construtor
        public ProdutoComposicaoValidator() {

            RuleFor(x => x.ProdutoComposicao.idProdutoItem)
                .NotEmpty().WithMessage("Informe o item.");

            RuleFor(x => x.ProdutoComposicao.idUnidadeMedida)
                .NotEmpty().WithMessage("Informe a unidade de medida.");

            RuleFor(x => x.ProdutoComposicao.valorUnidadeMedida)
                .NotEmpty().WithMessage("Informe o valor da unidade de medida.");

        }
        
    }
}
