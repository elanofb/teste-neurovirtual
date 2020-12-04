using DAL.ConfiguracoesAssociados;
using FluentValidation.Attributes;

namespace WEB.Areas.ConfiguracoesAssociados.ViewModels{

    [Validator(typeof(AssociadoCampoFormValidator))]
	public class AssociadoCampoForm {

        //Atributos

        //Servicos

        //Propriedades
        public ConfiguracaoAssociadoCampo AssociadoCampo { get; set; }

        public int? idCampoClone { get; set; }
       
        //Construtor
        public AssociadoCampoForm() { 

            this.AssociadoCampo = new ConfiguracaoAssociadoCampo();

        }
    }

}