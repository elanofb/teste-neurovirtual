using System;
using FluentValidation;
using BLL.Pessoas;

namespace WEB.Areas.Pessoas.ViewModels{

    //
    public class PessoaContatoFormValidator : AbstractValidator<PessoaContatoForm> {
        
		//Atributos
		private IPessoaContatoBL _PessoaContatoBL; 

		//Propriedades
		private IPessoaContatoBL OPessoaContatoBL { get{ return (this._PessoaContatoBL = this._PessoaContatoBL ?? new PessoaContatoBL() ); }}

        //Construtor
        public PessoaContatoFormValidator() {
            
            RuleFor(x => x.PessoaContato.idPessoa).NotEmpty().WithMessage("Informe a quem o contato deve estar vinculado.");

            RuleFor(x => x.PessoaContato.idTipoContato).GreaterThan(0).WithMessage("Informe qual é a área desse contato.");

            RuleFor(x => x.PessoaContato.nome).NotEmpty().WithMessage("Informe o nome do contato.");

			When(x => !String.IsNullOrEmpty(x.PessoaContato.nome), () => {

	            RuleFor(x => x.PessoaContato.nome)
					.Must((x, nome) => !this.existeContato(x) ).WithMessage("Já existe um contato cadastrado com esse nome.");
			});

        }

        //Verificar se o contato já existe
        public bool existeContato(PessoaContatoForm ViewModel) {

            int idDesconsiderado = UtilNumber.toInt32(ViewModel.PessoaContato.id);

			return this.OPessoaContatoBL.existe(ViewModel.PessoaContato.nome, ViewModel.PessoaContato.email, ViewModel.PessoaContato.idPessoa, idDesconsiderado);
        }

    }
}
