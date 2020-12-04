using DAL.Configuracoes;

namespace WEB.Areas.Configuracao.ViewModels{
    
	public class ConfiguracaoSaqueForm{

        //Atributos

        //Servicos

        //Propriedades
		public ConfiguracaoSaque ConfiguracaoSaque { get; set; }
		
        //Construtor
        public ConfiguracaoSaqueForm() { 

			this.ConfiguracaoSaque = new ConfiguracaoSaque();
        }
		
    }

}