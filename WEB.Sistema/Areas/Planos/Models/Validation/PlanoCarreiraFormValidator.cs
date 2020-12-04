using FluentValidation;
using BLL.Planos;

namespace WEB.Areas.Planos.ViewModels{

    //
    public class PlanoCarreiraFormValidator : AbstractValidator<PlanoCarreiraForm> {
        
		//Atributos
		private IPlanoCarreiraConsultaBL _PlanoCarreiraConsultaBL; 
        
		//Propriedades
		private IPlanoCarreiraConsultaBL OPlanoCarreiraConsultaBL { get{ return (this._PlanoCarreiraConsultaBL = this._PlanoCarreiraConsultaBL ?? new PlanoCarreiraConsultaBL() ); }}
        
        //Construtor
        public PlanoCarreiraFormValidator() {
            
            RuleFor(x => x.PlanoCarreira.descricao)
                .NotEmpty().WithMessage("A descrição do plano é obrigatória.")
                .Must((x, nome) => !this.registroJaExiste(x))
                .WithMessage("Já existe um registro cadastrado com esse nome.");
            
            RuleFor(x => x.PlanoCarreira.pontuacao).GreaterThan(0).WithMessage("Informe a pontuação do plano.");
        }
        
        private bool registroJaExiste(PlanoCarreiraForm VM) {
            
            return this.OPlanoCarreiraConsultaBL.existe(VM.PlanoCarreira.descricao, VM.PlanoCarreira.id);
            
        }
        
    }
}
