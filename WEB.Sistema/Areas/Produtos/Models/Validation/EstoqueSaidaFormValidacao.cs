using System;
using FluentValidation;
using BLL.Produtos;
using DAL.Produtos;

namespace WEB.Areas.Produtos.ViewModels{

    //
    public class EstoqueSaidaFormValidator : AbstractValidator<EstoqueSaidaForm> {
        
		//Atributos
		private IEstoqueSaidaBL _EstoqueSaidaBL; 
		private IProdutoBL _ProdutoBL; 

		//Propriedades
		private IEstoqueSaidaBL OEstoqueSaidaBL { get{ return (this._EstoqueSaidaBL = this._EstoqueSaidaBL ?? new EstoqueSaidaBL() ); }}
		private IProdutoBL OProdutoBL { get{ return (this._ProdutoBL = this._ProdutoBL ?? new ProdutoBL() ); }}

        //Construtor
        public EstoqueSaidaFormValidator() {

            RuleFor(x => x.EstoqueSaida.idReferencia)
				.NotEmpty()
				.WithMessage("Informe para onde está indo o produto.");

            RuleFor(x => x.EstoqueSaida.ProdutoEstoque.idProduto)
				.NotEmpty()
				.WithMessage("Informe o produto.");

            RuleFor(x => x.EstoqueSaida.ProdutoEstoque.dtMovimentacao)
				.NotEmpty()
				.WithMessage("Informe a data de saída do produto.");

            RuleFor(x => x.EstoqueSaida.ProdutoEstoque.qtdMovimentada)
				.NotEmpty()
				.WithMessage("Informe quantidade do produto.");

			RuleFor(x =>x.EstoqueSaida.idTipoReferenciaSaida)
					.Must( (x, idTipoReferenciaSaida) => !this.existe(x) )
					.WithMessage("Já existe uma saída de estoque cadastrada com essas informações.");

            When(x => (x.EstoqueSaida.ProdutoEstoque.idProduto > 0), () =>{
        	    RuleFor(x =>x.EstoqueSaida.ProdutoEstoque.idProduto)
			            .Must( (x, idProduto) => !this.temEstoque(x) )
					    .WithMessage("Estoque deste produto é inferior a quantidade solictada.");
			});
        }

        //Verificar se existe outra saida de estoque
        public bool existe(EstoqueSaidaForm ViewModel) {
            int idDesconsiderado = UtilNumber.toInt32(ViewModel.EstoqueSaida.id);
			return this.OEstoqueSaidaBL.existe(ViewModel.EstoqueSaida.ProdutoEstoque.dtMovimentacao, 
                                               ViewModel.EstoqueSaida.idTipoReferenciaSaida, 
                                               ViewModel.EstoqueSaida.idReferencia, 
                                               ViewModel.EstoqueSaida.ProdutoEstoque.idProduto, 
                                               ViewModel.EstoqueSaida.ProdutoEstoque.qtdMovimentada, 
                                               idDesconsiderado);
        }
        public bool temEstoque(EstoqueSaidaForm ViewModel) {

            Produto OProduto = OProdutoBL.carregar(ViewModel.EstoqueSaida.ProdutoEstoque.idProduto);
            if (OProduto.qtde < ViewModel.EstoqueSaida.ProdutoEstoque.qtdMovimentada) {
                return true;
            }

            return false;
        }
    }
}
