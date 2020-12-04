using FluentValidation;

namespace WEB.Areas.Configuracao.ViewModels {

    //
    public class ConfiguracoesVMValidator : AbstractValidator<ConfiguracoesVM> {

        //
        public ConfiguracoesVMValidator() {


	        RuleFor(m => m.ConfiguracaoNotificacao.corpoEmailNovoAssociado)
		        .Length(0, 3000).WithMessage("O e-mail para novos associados devem ter no máximo 3000 caracteres.");
            
        }

    }
}