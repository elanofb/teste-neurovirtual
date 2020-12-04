using System;
using FluentValidation;
using BLL.Produtos;

namespace WEB.Areas.Produtos.ViewModels{

    //
    public class EstoqueEntradaFormValidator : AbstractValidator<EstoqueEntradaForm> {
        
		//Atributos
		private IEstoqueEntradaBL _EstoqueEntradaBL; 

		//Propriedades
		private IEstoqueEntradaBL OEstoqueEntradaBL { get{ return (this._EstoqueEntradaBL = this._EstoqueEntradaBL ?? new EstoqueEntradaBL() ); }}

        //Construtor
        public EstoqueEntradaFormValidator() {

            RuleFor(x => x.EstoqueEntrada.idFornecedor)
				.NotEmpty()
				.WithMessage("Informe o fornecedor do produto.");

            RuleFor(x => x.EstoqueEntrada.ProdutoEstoque.idProduto)
				.NotEmpty()
				.WithMessage("Informe o fornecedor do produto.");

            RuleFor(x => x.EstoqueEntrada.ProdutoEstoque.dtMovimentacao)
				.NotEmpty()
				.WithMessage("Informe a data de entrada do produto.");

            RuleFor(x => x.EstoqueEntrada.ProdutoEstoque.qtdMovimentada)
				.NotEmpty()
				.WithMessage("Informe quantidade do produto.");

			RuleFor(x =>x.EstoqueEntrada.idFornecedor)
					.Must( (x, idFornecedor) => !this.existe(x) )
					.WithMessage("Já existe uma entrada de estoque cadastrado com essas informações.");
        }

        //Verificar se existe outra empresa com o mesmo CNPJ
        public bool existe(EstoqueEntradaForm ViewModel) {
            int idDesconsiderado = UtilNumber.toInt32(ViewModel.EstoqueEntrada.id);
			return this.OEstoqueEntradaBL.existe(ViewModel.EstoqueEntrada.ProdutoEstoque.dtMovimentacao, ViewModel.EstoqueEntrada.idFornecedor, ViewModel.EstoqueEntrada.ProdutoEstoque.idProduto, ViewModel.EstoqueEntrada.ProdutoEstoque.qtdMovimentada, idDesconsiderado);
        }
    }
}
