using DAL.ConfiguracoesAssociados;

namespace WEB.Areas.ConfiguracoesAssociados.ViewModels{

	public class ConfiguracaoAssociadoPFForm{

        //Atributos

        //Servicos

        //Propriedades
		public ConfiguracaoAssociadoPF ConfiguracaoAssociadoPF { get; set; }

        //Construtor
        public ConfiguracaoAssociadoPFForm() { 

			this.ConfiguracaoAssociadoPF = new ConfiguracaoAssociadoPF();
        }
    }

}