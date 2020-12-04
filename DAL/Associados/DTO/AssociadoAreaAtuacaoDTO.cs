using System;
using System.Collections.Generic;
using DAL.Arquivos;
using DAL.Contribuicoes;
using DAL.Localizacao;
using DAL.Mailings;
using DAL.MeiosDivulgacao;
using DAL.Organizacoes;
using DAL.Permissao;
using DAL.Pessoas;
using DAL.Tipos;
using DAL.Unidades;

namespace DAL.Associados {

	public class AssociadoAreaAtuacaoDTO {
		
		public int idAssociado { get; set; }
		
		public string nome { get; set; }
		
		public string nroDocumento { get; set; }
		
		public string tipoAssociado { get; set; }			
 		
		public List<string> listaAreaAtuacao { get; set; }

		public List<PessoaEmailResumoDTO> listaEmails { get; set; }

		public List<string> listaTelefones { get; set; }	
       

        //Construtor
		public AssociadoAreaAtuacaoDTO() {
	
			this.listaAreaAtuacao = new List<string>();
			
            this.listaEmails = new List<PessoaEmailResumoDTO>();

            this.listaTelefones = new List<string>();
            
		}
	}
}