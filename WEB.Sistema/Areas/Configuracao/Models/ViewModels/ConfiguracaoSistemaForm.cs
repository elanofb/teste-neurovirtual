using DAL.Configuracoes;
using FluentValidation.Attributes;

namespace WEB.Areas.Configuracao.ViewModels{

    [Validator(typeof(ConfiguracaoSistemaFormValidator))]
	public class ConfiguracaoSistemaForm{

        //Atributos

        //Servicos

        //Propriedades
		public ConfiguracaoSistema ConfiguracaoSistema { get; set; }

        //Construtor
        public ConfiguracaoSistemaForm() { 

			this.ConfiguracaoSistema = new ConfiguracaoSistema();
        }
    }

}