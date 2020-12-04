using System;
using FluentValidation;
using FluentValidation.Attributes;
using DAL.Anuidades;
using DAL.Associados;

namespace WEB.Areas.Associados.ViewModels{

	[Validator(typeof(AssociadoAnuidadeDetalheValidator))]
	public class AssociadoAnuidadeDetalheVM{

        public int id { get; set;}
        
		public int idAssociado { get; set;}
		public Associado Associado { get; set;}        
		public int idAnuidade { get; set;}
		public Anuidade Anuidade { get; set;}
	

        public decimal valor { get; set;}
        public DateTime dtVencimento { get; set;}
        public DateTime dtVencimentoAtual { get; set;}
        public DateTime? dtPagamento { get; set;}
        public DateTime dtCadastro { get; set;}

        public string flagPago { get; set;}
        public string flagIsento { get; set;}
		public string observacoes { get; set;}

        //public List<TituloCobrancaPagamento> listaPagamentos { get; set;}

        ////
        //public AssociadoAnuidadeDetalheVM() { 
        //    this.Associado = new Associado();
        //    this.Anuidade = new Anuidade();
        //    this.listaPagamentos = new List<TituloCobrancaPagamento>();
        //}
	}

	//
	internal class AssociadoAnuidadeDetalheValidator : AbstractValidator<AssociadoAnuidadeDetalheVM> {
		
		//
		public AssociadoAnuidadeDetalheValidator() {

            RuleFor(x => x.idAssociado)
				 .NotEmpty().WithMessage("Informe para quem é a anuidade!")
				 .GreaterThan(0).WithMessage("Informe para quem é a anuidade!");

            RuleFor(x => x.Associado)
				 .NotNull().WithMessage("Informe para quem é a anuidade!");

		 }


	}

}