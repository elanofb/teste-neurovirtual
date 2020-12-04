using System;
using BLL.Ajudas;
using FluentValidation;

namespace WEB.Areas.Ajudas.ViewModels{

    //
    public class AjudaModuloFormValidator : AbstractValidator<AjudaModuloForm> {
        
		//Atributos
		private IAjudaModuloBL _AjudaModuloBL;

		//Propriedades
		private IAjudaModuloBL OAjudaModuloBL { get{ return (this._AjudaModuloBL = this._AjudaModuloBL ?? new AjudaModuloBL() ); }}

        //Construtor
        public AjudaModuloFormValidator() {


            RuleFor(x => x.AjudaModulo.titulo)
               .NotEmpty()
               .WithMessage("Informe o titulo.");

            RuleFor(x => x.AjudaModulo.titulo)
                    .Must((x, titulo) => !this.existe(x))
                    .WithMessage("Já existe um módulo cadastrado com esse titulo.");

            RuleFor(x => x.AjudaModulo.descricao).NotEmpty().WithMessage("Informe a descrição.");

        }
        
        public bool existe(AjudaModuloForm ViewModel) {
            int idDesconsiderado = UtilNumber.toInt32(ViewModel.AjudaModulo.id);
			return this.OAjudaModuloBL.existe(ViewModel.AjudaModulo.titulo, idDesconsiderado);
        }

    }
}
