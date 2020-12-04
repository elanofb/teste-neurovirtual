using DAL.Configuracoes;

namespace WEB.Areas.Configuracao.ViewModels{
    
	public class ConfiguracaoOperacaoCompraForm{

        //Atributos

        //Servicos

        //Propriedades
		public ConfiguracaoOperacaoCompra ConfiguracaoOperacaoCompra { get; set; }
		
        //Construtor
        public ConfiguracaoOperacaoCompraForm() { 

			this.ConfiguracaoOperacaoCompra = new ConfiguracaoOperacaoCompra();
        }
    }

}