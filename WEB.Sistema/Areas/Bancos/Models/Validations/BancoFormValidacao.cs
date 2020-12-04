using System;
using FluentValidation;
using BLL.Bancos;

namespace WEB.Areas.Bancos.ViewModels{

    //
    public class BancoValidator : AbstractValidator<BancoForm> {
        
		//Atributos
		private IBancoBL _BancoBL; 

		//Propriedades
		private IBancoBL OBancoBL { get{ return (this._BancoBL = this._BancoBL ?? new BancoBL() ); }}

        //Construtor
        public BancoValidator() {


            RuleFor(x => x.Banco.nome)
               .NotEmpty()
               .WithMessage("Informe o nome do banco.");

            RuleFor(x => x.Banco.nome)
                    .Must((x,nome) => !this.existe(x))
                    .WithMessage("Já existe um banco cadastrado com esse nome.");

        }

        //Verificar se existe outra empresa com o mesmo CNPJ
        public bool existe(BancoForm ViewModel) {
            int idDesconsiderado = UtilNumber.toInt32(ViewModel.Banco.id);
			return this.OBancoBL.existe(ViewModel.Banco.descricao, ViewModel.Banco.nroBanco, idDesconsiderado);
        }

    }
}
