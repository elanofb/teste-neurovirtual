using DAL.ConfiguracoesRecibo;

namespace WEB.Areas.ConfiguracoesRecibo.ViewModels{

	public class ConfiguracaoReciboForm {

        //Atributos

        //Servicos

        //Propriedades
		public ConfiguracaoRecibo ConfiguracaoRecibo { get; set; }

        //Construtor
        public ConfiguracaoReciboForm() { 

			this.ConfiguracaoRecibo = new ConfiguracaoRecibo();
        }
    }

}