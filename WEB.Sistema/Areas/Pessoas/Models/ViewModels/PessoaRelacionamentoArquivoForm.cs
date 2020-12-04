using System.Collections.Generic;
using WEB.Areas.Pessoas.DTO;

namespace WEB.Areas.Pessoas.ViewModels {

	public class PessoaRelacionamentoArquivoForm {

		//Propriedades
		public List<ArquivosPessoaRelacionamentoDTO> OListaArquivosPessoaRelacionamento { get; set; }

		//Construtor
		public PessoaRelacionamentoArquivoForm() { 
			this.OListaArquivosPessoaRelacionamento = new List<ArquivosPessoaRelacionamentoDTO>();
		}
	}
}