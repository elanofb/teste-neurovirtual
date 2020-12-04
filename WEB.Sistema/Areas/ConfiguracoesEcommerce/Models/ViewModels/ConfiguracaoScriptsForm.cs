using DAL.ConfiguracoesEcommerce;

namespace WEB.Areas.ConfiguracoesEcommerce.ViewModels{

	public class ConfiguracaoEcommerceForm{

        //Atributos

        //Servicos

        //Propriedades
		public ConfiguracaoEcommerce ConfiguracaoEcommerce { get; set; }

        //Construtor
        public ConfiguracaoEcommerceForm() { 

			this.ConfiguracaoEcommerce = new ConfiguracaoEcommerce();
        }
    }

}