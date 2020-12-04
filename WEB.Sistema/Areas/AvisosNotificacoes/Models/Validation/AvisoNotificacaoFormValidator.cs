using System;
using FluentValidation;

namespace WEB.Areas.AvisosNotificacoes.ViewModels {

    public class AvisoNotificacaoFormValidator : AbstractValidator<AvisoNotificacaoForm> {

        public AvisoNotificacaoFormValidator() {
            
            RuleFor(x => x.ONotificacaoSistema.titulo)
                .NotEmpty().WithMessage("O título mensagem deve ser preenchida")
                .Length(3, 255).WithMessage("O título deve conter entre 3 e 255 caracteres.");
            
            When(x => !(x.ONotificacaoSistema.idTemplate > 0), () => {
                
                RuleFor(x => x.ONotificacaoSistema.notificacao)
                    .NotEmpty().WithMessage("O campo mensagem deve ser preenchida")
                    .MaximumLength(8000).WithMessage("A mensagem deve conter no máximo 8000 caracteres.");
                
            });
            
            RuleFor(x => x.ONotificacaoSistema.flagTodosAssociados)
                .NotEmpty().WithMessage("Informe se a notificação será enviada para todos os associados.");

            When(x => x.ONotificacaoSistema.flagMobile == true, () => {
                
                RuleFor(x => x.ONotificacaoSistema.tituloPush)
                    .NotEmpty().WithMessage("O título Push deve ser informado.")
                    .Length(2, 100).WithMessage("O título Push deve conter entre 2 e 100 caracteres.");
                
                RuleFor(x => x.ONotificacaoSistema.notificacaoPush)
                    .NotEmpty().WithMessage("A mensagem Push deve ser informada.")
                    .Length(2, 500).WithMessage("A mensagem Push deve conter entre 2 e 500 caracteres.");
                
            });

        }

        private bool conterAssociado(AvisoNotificacaoForm ViewModel) {
            return SessionNotificacoes.getListAssociadosEspecificos().Count > 0;
        }

        private bool conterUsuario(AvisoNotificacaoForm ViewModel) {
            return (SessionNotificacoes.getListUsuariosEspecificos().Count > 0 || SessionNotificacoes.getListPerfisEspecificos().Count > 0);
        }
    }
}