using DAL.Configuracoes;

namespace WEB.Areas.Configuracao.ViewModels{

    public class ConfiguracaoRotinaAutomaticaForm {

        //Propriedades
        public ConfiguracaoRotinaAutomatica ConfiguracaoRotinaAutomatica { get; set; }

        //Construtor
        public ConfiguracaoRotinaAutomaticaForm() { 
			this.ConfiguracaoRotinaAutomatica = new ConfiguracaoRotinaAutomatica();
        }
    }

}