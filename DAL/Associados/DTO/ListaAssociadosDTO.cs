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

	public class ListaAssociadosDTO {

		public List<Associado> listaAssociados { get; set; }       
		
        //Construtor
		public ListaAssociadosDTO() {
		
			this.listaAssociados = new List<Associado>();
		}
	}
}