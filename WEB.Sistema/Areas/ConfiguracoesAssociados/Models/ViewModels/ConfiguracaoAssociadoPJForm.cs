using DAL.ConfiguracoesAssociados;

namespace WEB.Areas.ConfiguracoesAssociados.ViewModels{

	public class ConfiguracaoAssociadoPJForm {

        //Atributos

        //Servicos

        //Propriedades
		public ConfiguracaoAssociadoPJ ConfiguracaoAssociadoPJ { get; set; }

        //Construtor
        public ConfiguracaoAssociadoPJForm() { 

			this.ConfiguracaoAssociadoPJ = new ConfiguracaoAssociadoPJ();
        }
    }

}