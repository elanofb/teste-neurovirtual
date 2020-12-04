using System;
using FluentValidation;
using BLL.Bancos;

namespace WEB.Areas.Bancos.ViewModels{

    //
    public class BancoFormValidator : AbstractValidator<BancoForm> {
        
		//Atributos
		private IBancoBL _BancoBL; 

		//Propriedades
		private IBancoBL OBancoBL { get{ return (this._BancoBL = this._BancoBL ?? new BancoBL() ); }}

        //Construtor
        public BancoFormValidator() {


            RuleFor(x => x.Banco.nome)
               .NotEmpty()
               .WithMessage("Informe o nome do banco.");

            RuleFor(x => x.Banco.nome)
                    .Must((x,nome) => !this.existe(x))
                    .WithMessage("Já existe um banco cadastrado com esse nome.");

            RuleFor(x => x.Banco.nroBanco).NotEmpty().WithMessage("Informe o numero do banco valido.");

        }

        //Verificar se existe outra empresa com o mesmo CNPJ
        public bool existe(BancoForm ViewModel) {
            int idDesconsiderado = UtilNumber.toInt32(ViewModel.Banco.id);
			return this.OBancoBL.existe(ViewModel.Banco.descricao, ViewModel.Banco.nroBanco, idDesconsiderado);
        }

    }
}
