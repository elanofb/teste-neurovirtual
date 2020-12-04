using FluentValidation;
using BLL.Relacionamentos;

namespace WEB.Areas.Relacionamentos.ViewModels{

	public class OcorrenciaRelacionamentoValidator : AbstractValidator<OcorrenciaRelacionamentoForm> {
		
        //Constantes

        // Atributos
        private IOcorrenciaRelacionamentoPadraoBL _OcorrenciaRelacionamentoPadraoBL { get; set; }

        // Propriedades
        private IOcorrenciaRelacionamentoPadraoBL OOcorrenciaRelacionamentoPadraoBL { get { return (this._OcorrenciaRelacionamentoPadraoBL = this._OcorrenciaRelacionamentoPadraoBL ?? new OcorrenciaRelacionamentoPadraoBL()); } }

		//
        public OcorrenciaRelacionamentoValidator(){

			 RuleFor(x => x.OcorrenciaRelacionamento.descricao)
				 .NotEmpty().WithMessage("O título da ocorrência é obrigatório.")
                 .Must((x, descricao) => !this.existe(x) )
				 .WithMessage("Já existe uma ocorrência cadastrada com esse título.") ;

		 }

		//
        private bool existe(OcorrenciaRelacionamentoForm ViewModel){ 
			return this.OOcorrenciaRelacionamentoPadraoBL.existe(ViewModel.OcorrenciaRelacionamento.descricao, ViewModel.OcorrenciaRelacionamento.id);
		}
	}
}