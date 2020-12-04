using System;
using FluentValidation;
using BLL.Financeiro;

namespace WEB.Areas.Financeiro.ViewModels{

    //
    public class FormaPagamentoValidator : AbstractValidator<FormaPagamentoForm> {
        
		//Atributos
		private IFormaPagamentoBL _FormaPagamentoBL; 

		//Propriedades
		private IFormaPagamentoBL OFormaPagamentoBL { get{ return (this._FormaPagamentoBL = this._FormaPagamentoBL ?? new FormaPagamentoBL() ); }}

        //Construtor
        public FormaPagamentoValidator() {
            
            RuleFor(x => x.FormaPagamento.descricao)
				.NotEmpty()
				.WithMessage("Informe a descrição da Forma de Pagamento.");

			RuleFor(x =>x.FormaPagamento.descricao)
					.Must( (x, descricao) => !this.existe(x) )
                    .WithMessage("Já existe uma Forma de Pagamento cadastrada com essa descrição.");

        }

        //Verificar se existe outra empresa com o mesmo CNPJ
        public bool existe(FormaPagamentoForm ViewModel) {
            int idDesconsiderado = UtilNumber.toInt32(ViewModel.FormaPagamento.id);
			return this.OFormaPagamentoBL.existe(ViewModel.FormaPagamento.descricao, idDesconsiderado);
        }

    }
}
