using FluentValidation;

namespace WEB.Areas.ConfiguracoesEtiquetas.ViewModels{

	public class ConfiguracaoEtiquetaFormValidator : AbstractValidator<ConfiguracaoEtiquetaForm>{
        public ConfiguracaoEtiquetaFormValidator(){
            
            RuleFor(x => x.ConfiguracaoEtiqueta.descricao).NotEmpty().WithMessage("Informe a descrição da configuração");
        }
    }
}