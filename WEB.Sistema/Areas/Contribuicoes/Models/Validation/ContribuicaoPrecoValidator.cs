using System;
using System.Linq;
using FluentValidation;
using BLL.Contribuicoes;

namespace WEB.Areas.Contribuicoes.ViewModels{

    //
    public class ContribuicaoPrecoValidator : AbstractValidator<ContribuicaoPrecoForm> {
        
		//Atributos
        private IContribuicaoPrecoBL _ContribuicaoPrecoBL; 

		//Propriedades
		private IContribuicaoPrecoBL OContribuicaoPrecoBL => this._ContribuicaoPrecoBL = this._ContribuicaoPrecoBL ?? new ContribuicaoPrecoBL();

        //Construtor
        public ContribuicaoPrecoValidator() {
            
            RuleFor(x => x.ContribuicaoPreco.idTabelaPreco)
				.NotEmpty().WithMessage("Informe a tabela de preço.");

            RuleFor(x => x.ContribuicaoPreco.idTipoAssociado)
                .NotEmpty().WithMessage("Informe a o tipo do associado.")
                .Must( (x, idTipoAssociado) => !this.existe(x)).WithMessage("Já existem valores configurados para esse tipo de associado.");

            When(x => x.ContribuicaoPreco.flagIsento == false, () => {

                RuleFor(x => x.ContribuicaoPreco.valorFinal)
                    .NotEmpty().WithMessage("Informe o valor da cobrança.")
                    .GreaterThan(0).WithMessage("O valor da cobrança deve ser maior do que zero.");
                
            });

        }

        //Verificar se existe outra contribuição com o mesmo título
        private bool existe(ContribuicaoPrecoForm ViewModel) {

            var flagExiste = this.OContribuicaoPrecoBL.listar(0, UtilNumber.toInt32(ViewModel.ContribuicaoPreco.idTabelaPreco), "S")
                                                      .Any(x => x.idTipoAssociado == ViewModel.ContribuicaoPreco.idTipoAssociado && x.id != ViewModel.ContribuicaoPreco.id);


            return flagExiste;
        }

    }
}
