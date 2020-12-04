using System;
using FluentValidation;
using BLL.Produtos;
using UTIL.Upload;

namespace WEB.Areas.Produtos.ViewModels {

    public class ProdutoFormValidator : AbstractValidator<ProdutoForm> {

        //Atributos
        private IProdutoBL _ProdutoBL;

        //Propriedades
        private IProdutoBL OProdutoBL => _ProdutoBL = _ProdutoBL ?? new ProdutoBL();

        //Construtor
        public ProdutoFormValidator() {

            RuleFor(x => x.Produto.idTipoProduto)
                .NotEmpty().WithMessage("Informe qual é o Tipo de Produto.");

            RuleFor(x => x.Produto.nome)
                .NotEmpty().WithMessage("Informe um nome para o Produto.");           

            RuleFor(x => x.Produto.nome)
                .Must((x, nome) => !this.existe(x))
                .WithMessage("Já existe um Produto cadastrado com esse nome.");
            
            When(x => (x.Produto.flagCalcularFrete == true && x.Produto.flagFreteGratis != true), () => {
                RuleFor(x => x.Produto.peso)
                    .GreaterThan(0).WithMessage("O Peso do produto é necessário para calcular o frete.");

            });
            RuleFor(x => x.Produto.descricaoResumida).Length(0, 255).WithMessage("A descrição deve ser preenchida para detalhar o produto max. 255 caracteres.");

            //RuleFor(x => x.Produto.descricaoCompleta)
            //	.Length(0, 1000)
            //	.WithMessage("A descrição completa deve ter no máximo 1000 caracteres.");

            //When(x => (x.Produto.flagCortesia == true), () => {
            //    RuleFor(x => x.Produto.valor)
            //        .Equal(new Decimal(0))
            //        .WithMessage("Para produtos de cortesia, o valor deve ser 0,00.");
            //});

            When(x => x.Produto.flagCortesia != true, () => {
                RuleFor(x => x.Produto.valor)
                    .GreaterThan(new Decimal(0)).WithMessage("Informe o valor do produto.");
            });

            When(x => (x.OImagem != null), () => {
                RuleFor(x => x.OImagem)
                    .Must((x, OImagem) => UploadConfig.validarImagem(OImagem)).WithMessage("Envie uma imagem válida.");
            });

        }

        //Verificar se existe outra empresa com o mesmo CNPJ
        private bool existe(ProdutoForm ViewModel) {

            int idDesconsiderado = UtilNumber.toInt32(ViewModel.Produto.id);

            return this.OProdutoBL.existe(ViewModel.Produto.nome, idDesconsiderado);
        }

    }
}
