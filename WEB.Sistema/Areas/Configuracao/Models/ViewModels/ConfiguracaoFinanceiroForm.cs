using DAL.Configuracoes;

namespace WEB.Areas.Configuracao.ViewModels{

    public class ConfiguracaoFinanceiroForm {

        //Propriedades
        public ConfiguracaoFinanceiro ConfiguracaoFinanceiro { get; set; }

        //Construtor
        public ConfiguracaoFinanceiroForm() { 
			this.ConfiguracaoFinanceiro = new ConfiguracaoFinanceiro();
        }
    }

}