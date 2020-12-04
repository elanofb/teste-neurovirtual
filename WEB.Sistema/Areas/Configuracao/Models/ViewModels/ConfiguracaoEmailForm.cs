using DAL.Configuracoes;

namespace WEB.Areas.Configuracao.ViewModels{

    public class ConfiguracaoEmailForm {

        //Propriedades
        public ConfiguracaoEmail ConfiguracaoEmail { get; set; }

        //Construtor
        public ConfiguracaoEmailForm() { 
			this.ConfiguracaoEmail = new ConfiguracaoEmail();
        }
    }

}