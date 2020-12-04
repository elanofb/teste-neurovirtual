using System;
using System.Collections.Generic;
using FluentValidation;
using FluentValidation.Attributes;
using DAL.Localizacao;
using DAL.Pessoas;

namespace WEB.Areas.Pessoas.ViewModels{

	[Validator(typeof(PessoaValidator))]
	public class PessoaVM{

        public int id { get; set; }
        public string nome { get; set; }
        public string cpf { get; set; }
        public string rg { get; set; }
        public string orgaoEmissorRg { get; set; }
        
		public string idPaisNascimento { get; set; }
		public virtual Pais PaisNascimento  { get; set; }

		public int? idCidadeNascimento { get; set; }
		public virtual Cidade CidadeNascimento  { get; set; }
        public string nomeLocalNascimento { get; set; }

        public DateTime? dtNascimento { get; set; }
        public string flagSexo { get; set; }

        public string ddiTelPrincipal { get; set; }
        public string dddTelPrincipal { get; set; }
        public string nroTelPrincipal { get; set; }

        public string ddiTelSecundario { get; set; }
        public string dddTelSecundario { get; set; }
		public string nroTelSecundario { get; set; }

        public string ddiTelTerciario { get; set; }
        public string dddTelTerciario { get; set; }
        public string nroTelTerciario { get; set; }

		public string emailPrincipal { get; set; }
		public string emailSecundario { get; set; }

        public string nomePai { get; set; }
        public string nomeMae { get; set; }
        
        public string grauEscolaridade { get; set; }
        
        public string login { get; set; }
        public string senha { get; set; }

		public virtual List<PessoaEndereco> listaEnderecos { get; set;}

		//
		public PessoaVM(){
			
		}
	}

	//
	internal class PessoaValidator : AbstractValidator<PessoaVM> {
		
		//
		public PessoaValidator() {


		 }

    }
}