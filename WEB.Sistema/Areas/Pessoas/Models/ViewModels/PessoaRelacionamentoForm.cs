using System.Collections.Generic;
using DAL.Arquivos;
using DAL.Pessoas;
using FluentValidation.Attributes;

namespace WEB.Areas.Pessoas.ViewModels {

	[Validator(typeof(PessoaRelacionamentoFormValidator))]
	public class PessoaRelacionamentoForm {

		//Propriedades
		public PessoaRelacionamento PessoaRelacionamento { get; set; }
		public List<ArquivoDTO> Arquivos { get; set; }
		
		public bool flagRecarregar { get; set; }

		//Construtor
		public PessoaRelacionamentoForm() { 
			this.PessoaRelacionamento = new PessoaRelacionamento();
			this.Arquivos = new List<ArquivoDTO>();
		}

	}
}