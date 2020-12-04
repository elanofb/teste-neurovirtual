using System;
using BLL.Atendimentos;
using FluentValidation;

namespace WEB.Areas.Atendimentos.ViewModels{

    //
    public class AtendimentoTipoFormValidator : AbstractValidator<AtendimentoTipoForm> {
        
		//Atributos
		private IAtendimentoTipoBL _AtendimentoTipoBL;

        //Propriedades
        private IAtendimentoTipoBL OAtendimentoTipoBL => _AtendimentoTipoBL = _AtendimentoTipoBL ?? new AtendimentoTipoBL();

        //Construtor
        public AtendimentoTipoFormValidator() {
            
            RuleFor(x => x.AtendimentoTipo.descricao)
				.NotEmpty()
				.WithMessage("Informe a descrição.");

			RuleFor(x =>x.AtendimentoTipo.descricao)
					.Must( (x, descricao) => !this.existe(x) )
                    .WithMessage("Já existe um registro cadastrado com essa descrição.");

        }

        public bool existe(AtendimentoTipoForm ViewModel) {

            int idDesconsiderado = UtilNumber.toInt32(ViewModel.AtendimentoTipo.id);

			return this.OAtendimentoTipoBL.existe(ViewModel.AtendimentoTipo.descricao, idDesconsiderado);

        }

    }
}
