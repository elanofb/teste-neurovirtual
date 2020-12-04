using System;
using FluentValidation;

namespace WEB.Areas.AssociadosNotificacoes.ViewModels{

    //
    public class AssociadoNotificacaoFormValidator : AbstractValidator<AssociadoNotificacaoForm> {
        
        //Construtor
        public AssociadoNotificacaoFormValidator() {
            
            RuleFor(x => x.ONotificacao.dtProgramacaoEnvio)
				.NotEmpty().WithMessage("Informe a data em que o associado será notificado.")
                .GreaterThanOrEqualTo(DateTime.Today).WithMessage("A data de envio não pode ser inferior a data de hoje.");

            RuleFor(x => x.ONotificacao.titulo)
				.NotEmpty().WithMessage("Informe o título da mensagem.");

            RuleFor(x => x.ONotificacao.notificacao)
				.NotEmpty().WithMessage("Informe a mensagem que os associados receberão.");

            
        }
        

    }
}
