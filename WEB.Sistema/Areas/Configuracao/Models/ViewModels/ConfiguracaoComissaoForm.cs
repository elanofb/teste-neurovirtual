using DAL.Configuracoes;

namespace WEB.Areas.Configuracao.ViewModels{

    public class ConfiguracaoComissaoForm {

        //Propriedades
        public ConfiguracaoComissao ConfiguracaoComissao { get; set; }

        //Construtor
        public ConfiguracaoComissaoForm() { 
			this.ConfiguracaoComissao = new ConfiguracaoComissao();
        }
    }

}