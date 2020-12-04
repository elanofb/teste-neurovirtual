using DAL.ConfiguracoesCateirinha;

namespace WEB.Areas.ConfiguracoesCarteirinha.ViewModels{

	public class ConfiguracaoCarteirinhaForm {

        //Atributos

        //Servicos

        //Propriedades
		public ConfiguracaoCarteirinha ConfiguracaoCarteirinha { get; set; }

        //Construtor
        public ConfiguracaoCarteirinhaForm() { 

			this.ConfiguracaoCarteirinha = new ConfiguracaoCarteirinha();
        }
    }

}