using System;
using BLL.Atendimentos;
using FluentValidation;

namespace WEB.Areas.Atendimentos.ViewModels{

    //
    public class AtendimentoAreaFormValidator : AbstractValidator<AtendimentoAreaForm> {
        
		//Atributos
		private IAtendimentoAreaBL _AtendimentoAreaBL;

        //Propriedades
        private IAtendimentoAreaBL OAtendimentoAreaBL => _AtendimentoAreaBL = _AtendimentoAreaBL ?? new AtendimentoAreaBL();

        //Construtor
        public AtendimentoAreaFormValidator() {
            
            RuleFor(x => x.AtendimentoArea.descricao)
				.NotEmpty()
				.WithMessage("Informe a descrição.");

			RuleFor(x =>x.AtendimentoArea.descricao)
					.Must( (x, descricao) => !this.existe(x) )
                    .WithMessage("Já existe um registro cadastrado com essa descrição.");

        }

        public bool existe(AtendimentoAreaForm ViewModel) {

            int idDesconsiderado = UtilNumber.toInt32(ViewModel.AtendimentoArea.id);

			return this.OAtendimentoAreaBL.existe(ViewModel.AtendimentoArea.descricao, idDesconsiderado);

        }

    }
}
