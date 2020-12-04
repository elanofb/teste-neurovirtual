using System;
using FluentValidation;
using BLL.Produtos;

namespace WEB.Areas.Produtos.ViewModels{

    //
    public class TipoProdutoValidator : AbstractValidator<TipoProdutoForm> {
        
		//Atributos
		private ITipoProdutoBL _TipoProdutoBL; 

		//Propriedades
		private ITipoProdutoBL OTipoProdutoBL => _TipoProdutoBL = _TipoProdutoBL ?? new TipoProdutoBL();

        //Construtor
        public TipoProdutoValidator() {
            
            RuleFor(x => x.TipoProduto.descricao)
				.NotEmpty().WithMessage("Informe a descrição do Tipo de Produto.");

			RuleFor(x =>x.TipoProduto.descricao)
					.Must( (x, descricao) => !this.existe(x) )
					.WithMessage("Já existe um Tipo de Produto cadastrado com essa descrição.");

            When(x => x.TipoProduto.flagServico != true, () => {

                RuleFor(x => x.TipoProduto.flagProduto)
                    .Equal(true).WithMessage("Esse tipo de produto deve ser 'Produto' quando o tipo 'Serviço' estiver desabilitado.");

            });

            When(x => x.TipoProduto.flagProduto != true, () => {

                RuleFor(x => x.TipoProduto.flagServico)
                    .Equal(true).WithMessage("Esse tipo de produto deve ser 'Serviço' quando o tipo 'Produto' estiver desabilitado.");

            });

        }

        //Verificar se existe outra empresa com o mesmo CNPJ
        public bool existe(TipoProdutoForm ViewModel) {
            int idDesconsiderado = UtilNumber.toInt32(ViewModel.TipoProduto.id);
			return this.OTipoProdutoBL.existe(ViewModel.TipoProduto.descricao, idDesconsiderado);
        }

    }
}
