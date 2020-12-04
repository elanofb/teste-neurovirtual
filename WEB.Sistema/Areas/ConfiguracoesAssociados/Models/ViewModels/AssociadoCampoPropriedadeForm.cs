using DAL.ConfiguracoesAssociados;
using FluentValidation.Attributes;

namespace WEB.Areas.ConfiguracoesAssociados.ViewModels{

    [Validator(typeof(AssociadoCampoPropriedadeFormValidator))]
	public class AssociadoCampoPropriedadeForm {

        //Propriedades
        public ConfiguracaoAssociadoCampoPropriedade AssociadoCampoPropriedade { get; set; }

        //Construtor
        public AssociadoCampoPropriedadeForm() { 
            this.AssociadoCampoPropriedade = new ConfiguracaoAssociadoCampoPropriedade();
        }
    }
}