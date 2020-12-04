using DAL.ConfiguracoesScripts;

namespace WEB.Areas.ConfiguracoesScripts.ViewModels{

	public class ConfiguracaoScriptsForm{

        //Atributos

        //Servicos

        //Propriedades
		public ConfiguracaoScripts ConfiguracaoScripts { get; set; }

        //Construtor
        public ConfiguracaoScriptsForm() { 

			this.ConfiguracaoScripts = new ConfiguracaoScripts();
        }
    }

}