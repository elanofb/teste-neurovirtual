using DAL.ConfiguracoesAssociados;
using FluentValidation.Attributes;

namespace WEB.Areas.ConfiguracoesAssociados.ViewModels{

    [Validator(typeof(AssociadoCampoGrupoFormValidator))]
	public class AssociadoCampoGrupoForm {

        //Atributos

        //Servicos

        //Propriedades
        public ConfiguracaoAssociadoCampoGrupo AssociadoCampoGrupo { get; set; }

        //Construtor
        public AssociadoCampoGrupoForm() { 

            this.AssociadoCampoGrupo = new ConfiguracaoAssociadoCampoGrupo();

        }
    }

}