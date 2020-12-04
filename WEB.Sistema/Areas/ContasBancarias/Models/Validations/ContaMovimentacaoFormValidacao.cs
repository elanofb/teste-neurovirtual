using FluentValidation;
using BLL.ContasBancarias;

namespace WEB.Areas.ContasBancarias.ViewModels{

    //
    public class ContaMovimentacaoValidator : AbstractValidator<ContaMovimentacaoForm> {
        
		//Atributos
		private IContaBancariaMovimentacaoBL _ContaMovimentacaoBL; 

		//Propriedades
		private IContaBancariaMovimentacaoBL OContaMovimentacaoBL { get{ return (this._ContaMovimentacaoBL = this._ContaMovimentacaoBL ?? new ContaBancariaMovimentacaoBL() ); }}

        //Construtor
        public ContaMovimentacaoValidator() {
            RuleFor(x => x.ContaMovimentacao.dtOperacao).NotEmpty().WithMessage("Digite da operação");
            RuleFor(x => x.ContaMovimentacao.valor).NotEmpty().WithMessage("Digite o valor");
        }
        
        public bool existeContaAgencia(ContaMovimentacaoForm ViewModel) {
            return this.OContaMovimentacaoBL.existe(ViewModel.ContaMovimentacao,false);
        }

        public bool existe(ContaMovimentacaoForm ViewModel) {
            return this.OContaMovimentacaoBL.existe(ViewModel.ContaMovimentacao,true);
        }
    }
}
