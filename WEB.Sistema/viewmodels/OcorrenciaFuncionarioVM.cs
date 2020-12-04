using System;
using FluentValidation;
using FluentValidation.Attributes;

namespace WEB.ViewModels{

    [Validator(typeof(OcorrenciaFuncionarioValidator))]
	public class OcorrenciaFuncionarioVM{
		public int id { get; set;}
		public string descricao { get; set;}
		public string ativo { get; set;}
		public DateTime dtCadastro { get; set;}
		public DateTime dtAlteracao { get; set;}
		public string flagSistema { get; set;}
	}

	//
	internal class OcorrenciaFuncionarioValidator : AbstractValidator<OcorrenciaFuncionarioVM> {
		
		//
        public OcorrenciaFuncionarioValidator(){
			 RuleFor(x => x.descricao)
				 .NotEmpty().WithMessage("Esse campo é obrigatório.")
				 .WithMessage("Já existe um registro cadastrado com essa descrição.") ;

		 }

		//
	}
}