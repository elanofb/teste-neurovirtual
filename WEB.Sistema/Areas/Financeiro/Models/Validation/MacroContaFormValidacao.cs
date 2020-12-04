using System;
using FluentValidation;
using BLL.Financeiro;


namespace WEB.Areas.Financeiro.ViewModels{

    //
    public class MacroContaValidator : AbstractValidator<MacroContaForm> {
        
		//Atributos
		private IMacroContaBL _MacroContaBL; 

		//Propriedades
		private IMacroContaBL OMacroContaBL { get{ return (this._MacroContaBL = this._MacroContaBL ?? new MacroContaBL() ); }}

        //Construtor
        public MacroContaValidator() {
            
            RuleFor(x => x.MacroConta.descricao)
				.NotEmpty()
				.WithMessage("Informe a descrição da Macro Conta.");

			RuleFor(x =>x.MacroConta.descricao)
					.Must( (x, descricao) => !this.existe(x) )
                    .WithMessage("Já existe uma Macro Conta com essa descrição.");

        }

        //Verificar se existe outra empresa com o mesmo CNPJ
        public bool existe(MacroContaForm ViewModel) {
            int idDesconsiderado = UtilNumber.toInt32(ViewModel.MacroConta.id);
			return this.OMacroContaBL.existe(ViewModel.MacroConta.descricao, idDesconsiderado);
        }

    }
}
