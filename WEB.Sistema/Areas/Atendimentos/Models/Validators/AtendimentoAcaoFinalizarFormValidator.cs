using FluentValidation;

namespace WEB.Areas.Atendimentos.ViewModels{

    //
    public class AtendimentoAcaoFinalizarFormValidator : AbstractValidator<AtendimentoAcaoFinalizarForm> {
        
        //Construtor
        public AtendimentoAcaoFinalizarFormValidator() {
            
            When(x => x.AtendimentoHistorico.flagAtendido == false, () => {

                RuleFor(x => x.AtendimentoHistorico.mensagem)
                    .NotEmpty().WithMessage("Informe o motivo pelo qual o atendimento não pôde ser atendido.")
                    .MaximumLength(500).WithMessage("Essa informação não pode ter mais de 500 caracteres");

            });

        }
        
    }

}
