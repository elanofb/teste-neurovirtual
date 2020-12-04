using DAL.Configuracoes;
using FluentValidation.Attributes;

namespace WEB.Areas.Configuracao.ViewModels{

    [Validator(typeof(ConfiguracaoContribuicaoFormValidator))]
    public class ConfiguracaoContribuicaoForm {

        //Propriedades
        public ConfiguracaoContribuicao ConfiguracaoContribuicao { get; set; }

        //Construtor
        public ConfiguracaoContribuicaoForm() { 
			this.ConfiguracaoContribuicao = new ConfiguracaoContribuicao();
        }
    }

}