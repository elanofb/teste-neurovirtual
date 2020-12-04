using FluentValidation.Attributes;
using DAL.Associados;
using System;
using BLL.ConfiguracoesAssociados;
using DAL.ConfiguracoesAssociados;

namespace WEB.Areas.Associados.ViewModels{

	[Validator(typeof(AssociadoFormValidator))]
	public class AssociadoForm{

		//Atributos
		
		//Servicos

		//Propriedades
		public ConfiguracaoAssociadoPF ConfiguracaoAssociadoPF { get; set; }

		public ConfiguracaoAssociadoPJ ConfiguracaoAssociadoPJ { get; set; }

        public Associado Associado { get; set; }

	    //Construtor
		public AssociadoForm(){
		}

		//Atribuir valores padrão para quando estiverem em branco
		public void configurarValoresPadrao() { 

			this.Associado.Pessoa.idPaisOrigem = !String.IsNullOrEmpty(this.Associado.Pessoa.idPaisOrigem)? this.Associado.Pessoa.idPaisOrigem: "BRA";

		    this.ConfiguracaoAssociadoPF = ConfiguracaoAssociadoPFBL.getInstance.carregar();

            this.ConfiguracaoAssociadoPJ = ConfiguracaoAssociadoPJBL.getInstance.carregar();

		}

	}

}