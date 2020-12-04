using System.Linq;
using FluentValidation;
using BLL.Contribuicoes;

namespace WEB.Areas.Contribuicoes.ViewModels{

    //
    public class ContribuicaoTabelaPrecoValidator : AbstractValidator<ContribuicaoTabelaPrecoForm> {
        
		//Atributos
		private IContribuicaoTabelaPrecoBL _ContribuicaoTabelaPrecoBL; 

		//Propriedades
		private IContribuicaoTabelaPrecoBL OContribuicaoTabelaPrecoBL => this._ContribuicaoTabelaPrecoBL = this._ContribuicaoTabelaPrecoBL ?? new ContribuicaoTabelaPrecoBL();

        //Construtor
        public ContribuicaoTabelaPrecoValidator() {
            
            RuleFor(x => x.ContribuicaoTabelaPreco.descricao)
				.NotEmpty().WithMessage("Informe a descrição da tabela.");

            RuleFor(x => x.ContribuicaoTabelaPreco.dtInicioVigencia)
            	.NotEmpty().WithMessage("Informe a data de início da vigência.")
                //.GreaterThanOrEqualTo(DateTime.Today).WithMessage("A data de vigência deve ser maior ou igual a hoje.")
                .Must( (x, dtInicioVigencia) => !this.existeData(x)).WithMessage("Já existe uma tabela configurada para essa data.");

        }

        //Verificar se existe outra contribuição com o mesmo título
        private bool existeData(ContribuicaoTabelaPrecoForm ViewModel) {

            bool flagExiste = this.OContribuicaoTabelaPrecoBL.listar(ViewModel.ContribuicaoTabelaPreco.idContribuicao, true)
                                  .Any(x => x.dtInicioVigencia == ViewModel.ContribuicaoTabelaPreco.dtInicioVigencia &&
                                            x.id != ViewModel.ContribuicaoTabelaPreco.id);

            return flagExiste;
        }

    }
}
