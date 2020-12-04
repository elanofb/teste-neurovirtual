using DAL.ConfiguracoesAssociados;
using FluentValidation.Attributes;

namespace WEB.Areas.ConfiguracoesAssociados.ViewModels{

    [Validator(typeof(AssociadoCampoOpcaoFormValidator))]
	public class AssociadoCampoOpcaoForm {

        //Opcaos
        public ConfiguracaoAssociadoCampoOpcao AssociadoCampoOpcao { get; set; }

        //Construtor
        public AssociadoCampoOpcaoForm() { 
            this.AssociadoCampoOpcao = new ConfiguracaoAssociadoCampoOpcao();
        }
    }
}