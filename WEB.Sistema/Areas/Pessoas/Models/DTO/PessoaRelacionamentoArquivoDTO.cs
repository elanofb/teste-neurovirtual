using System;
using System.Collections.Generic;
using DAL.Arquivos;
using DAL.Pessoas;

namespace WEB.Areas.Pessoas.DTO {

	public class PessoaRelacionamentoDTO{

		//Propriedades
		public PessoaRelacionamento PessoaRelacionamento { get; set; }
		public List<ArquivoUpload> OListaArquivos { get; set; }		
	}
	
	public class ArquivosPessoaRelacionamentoDTO{

		//Propriedades
		public ArquivoUpload OArquivoUpload { get; set; }		
		public string descOcorrencia { get; set; }
		public DateTime dtOcorrencia { get; set; }
		public string nomeUsuarioCadastro { get; set; }
	}
}