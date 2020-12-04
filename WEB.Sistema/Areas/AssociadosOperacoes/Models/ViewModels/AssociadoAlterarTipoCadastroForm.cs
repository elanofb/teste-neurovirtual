using System;
using System.Collections.Generic;
using DAL.Associados;
using DAL.Associados.DTO;
using FluentValidation.Attributes;

namespace WEB.Areas.AssociadosOperacoes.ViewModels{
    
    [Validator(typeof(AssociadoAlterarTipoCadastroFormValidator))]
	public class AssociadoAlterarTipoCadastroForm {

		//Atributos
		
		//Propriedades
        public List<ItemListaAssociado> listaAssociados { get; set; }

		public List<int> idsAssociados { get; set; }
        
        public byte? idTipoCadastro { get; set; }
        public int? idTipoAssociado { get; set; }
		
	    //Construtor
		public AssociadoAlterarTipoCadastroForm(){

		}

	    public void preencherTipoAssociado(){
			
		    if (this.idTipoCadastro.toInt() == 0){
			    return;
		    }

		    if (this.idTipoCadastro == AssociadoTipoCadastroConst.CONSUMIDOR){
			    this.idTipoAssociado = TipoAssociadoConst.CONSUMIDOR;
		    }
		    
		    if (this.idTipoCadastro == AssociadoTipoCadastroConst.COMERCIANTE){
			    this.idTipoAssociado = TipoAssociadoConst.COMERCIANTE;
		    }

	    }
        
	}

}