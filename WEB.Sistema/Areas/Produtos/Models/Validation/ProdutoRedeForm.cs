using System;
using System.Linq;
using FluentValidation;
using BLL.Produtos;

namespace WEB.Areas.Produtos.ViewModels{

    //
    public class ProdutoRedeFormValidator : AbstractValidator<ProdutoRedeForm> {
        
		//Atributos
		private IProdutoRedeConfiguracaoConsultaBL _ConsultaBL; 

		//Propriedades
		private IProdutoRedeConfiguracaoConsultaBL ConsultaBL => _ConsultaBL = _ConsultaBL ?? new ProdutoRedeConfiguracaoConsultaBL();

        //Construtor
        public ProdutoRedeFormValidator() {
            
            RuleFor(x => x.ProdutoRede.idProduto)
               .NotEmpty().WithMessage("É obrigatório informar o produto vinculado.")
               .GreaterThan(0).WithMessage("É obrigatório informar o produto vinculado.");

			RuleFor(x => x.ProdutoRede.nivel)
			.NotEmpty().WithMessage("É obrigatório informar o nível da rede.")
			.GreaterThan((byte)0).WithMessage("o nível deve ser maior do que zero.");

			RuleFor(x => x.ProdutoRede.percentualComissao)
			.GreaterThan((byte)0).WithMessage("o percentual de comissão deve ser maior do que zero.")
			.LessThan((byte)50).WithMessage("o percentual de comissão não pode ser tão alto.");

			RuleFor(x =>x.ProdutoRede.nivel)
					.Must( (x, nivel) => !this.existe(x) )
					.WithMessage("Já existe uma configuração para o nível informado.");


        }

        //Verificar se existe outra empresa com o mesmo CNPJ
        public bool existe(ProdutoRedeForm ViewModel) {
			
            int idDesconsiderado = UtilNumber.toInt32(ViewModel.ProdutoRede.id);
			
			var flagExiste = this.ConsultaBL.query().Any(x => x.idProduto == ViewModel.ProdutoRede.idProduto && x.nivel == ViewModel.ProdutoRede.nivel && x.id != idDesconsiderado); 
			
			return flagExiste;
        }

    }
}
