using DAL.Configuracoes;

namespace WEB.Areas.Configuracao.ViewModels{
    
    public class ConfiguracaoNotificacaoForm {
        
        //Propriedades
        public ConfiguracaoNotificacao ConfiguracaoNotificacao { get; set; }
        
        //Construtor
        public ConfiguracaoNotificacaoForm() { 
			this.ConfiguracaoNotificacao = new ConfiguracaoNotificacao();
        }
    }

}