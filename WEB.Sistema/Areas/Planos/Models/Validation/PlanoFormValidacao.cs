using FluentValidation;
using BLL.Planos;

namespace WEB.Areas.Planos.ViewModels{

    //
    public class PlanoFormValidator : AbstractValidator<PlanoForm> {
        
		//Atributos
		private IPlanoBL _PlanoBL; 

		//Propriedades
		private IPlanoBL OPlanoBL { get{ return (this._PlanoBL = this._PlanoBL ?? new PlanoBL() ); }}

        //Construtor
        public PlanoFormValidator() {

            RuleFor(x => x.Plano.nome)
                .NotEmpty().WithMessage("O nome do plano é obrigatório.")
                .Must((x, nome) => !this.registroJaExiste(x))
                .WithMessage("Já existe um registro cadastrado com esse nome.");

            RuleFor(x => x.Plano.qtdeMesVigencia).NotEmpty().WithMessage("Informe a vigência do plano.");
        }

        private bool registroJaExiste(PlanoForm VM) {
            return this.OPlanoBL.existe(VM.Plano.nome, VM.Plano.id);
        }
    }
}
