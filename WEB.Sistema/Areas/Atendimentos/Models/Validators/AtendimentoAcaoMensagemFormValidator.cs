using FluentValidation;

namespace WEB.Areas.Atendimentos.ViewModels{

    //
    public class AtendimentoAcaoMensagemFormValidator : AbstractValidator<AtendimentoAcaoMensagemForm> {
        
        //Construtor
        public AtendimentoAcaoMensagemFormValidator() {
            
            RuleFor(x => x.AtendimentoHistorico.mensagem)
				.NotEmpty().WithMessage("Essa informação é obrigatória.")
                .MaximumLength(500).WithMessage("Essa informação não pode ter mais de 500 caracteres");

        }
        
    }

}
