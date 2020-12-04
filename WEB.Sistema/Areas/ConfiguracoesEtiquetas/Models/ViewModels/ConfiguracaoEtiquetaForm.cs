using DAL.ConfiguracoesEtiquetas;
using FluentValidation.Attributes;

namespace WEB.Areas.ConfiguracoesEtiquetas.ViewModels{

    [Validator(typeof(ConfiguracaoEtiquetaFormValidator))]
	public class ConfiguracaoEtiquetaForm {
        
        //Propriedades
		public ConfiguracaoEtiqueta ConfiguracaoEtiqueta { get; set; }

        //Construtor
        public ConfiguracaoEtiquetaForm() { 
			this.ConfiguracaoEtiqueta = new ConfiguracaoEtiqueta();
        }
    }
}