using System;
using FluentValidation;
using UTIL.Resources;

namespace WEB.Areas.Permissao.ViewModels {

	public class AcessoRecursoValidator : AbstractValidator<AcessoRecursoForm> {

		public AcessoRecursoValidator() {
			RuleFor(x => x.idRecursoGrupo)
				.NotEmpty().WithMessage(String.Format(ValidationMessages.Required, "Grupo"));

			RuleFor(x => x.controller)
				.NotEmpty().WithMessage(String.Format(ValidationMessages.Required, "controller"));

			RuleFor(x => x.descricao)
				.NotEmpty().WithMessage(String.Format(ValidationMessages.Required, "descricao"));
		}
	}
}