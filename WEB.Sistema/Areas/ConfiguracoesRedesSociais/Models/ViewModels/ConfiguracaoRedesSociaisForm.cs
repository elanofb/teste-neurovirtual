using DAL.ConfiguracoesRedesSociais;

namespace WEB.Areas.ConfiguracoesRedesSociais.ViewModels{

	public class ConfiguracaoRedesSociaisForm {

        //Atributos

        //Servicos

        //Propriedades
		public ConfiguracaoRedesSociais ConfiguracaoRedesSociais { get; set; }

        //Construtor
        public ConfiguracaoRedesSociaisForm() { 

			this.ConfiguracaoRedesSociais = new ConfiguracaoRedesSociais();

        }
    }

}