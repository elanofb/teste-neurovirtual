using DAL.Configuracoes;
using FluentValidation;
using FluentValidation.Attributes;
using DAL.LogsPermissao;

namespace WEB.Areas.Permissao.ViewModels{

	[Validator(typeof(LoginValidator))]
	public class LoginVM{

        public LogUsuarioSistemaAcesso LogUsuariosistemaAcesso { get; set; }

        public ConfiguracaoSistema OConfigSistema { get; set; }

        public string login {get; set;}
		
		public string senha {get; set;}

		public string returnUrl {get; set;}
	}

	//
	internal class LoginValidator : AbstractValidator<LoginVM> {

		//
		public LoginValidator() {

			RuleFor(x => x.login)
				.NotEmpty().WithMessage("Informe qual é o login.");

            RuleFor(x => x.senha)
                .NotEmpty().WithMessage("Informe a sua senha.");

		}

	}
}
