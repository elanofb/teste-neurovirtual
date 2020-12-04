using System.Linq;
using BLL.ConfiguracoesTextos;
using FluentValidation;

namespace WEB.Areas.ConfiguracoesTextos.ViewModels {
    
    public class ConfiguracaoTextoFormValidator : AbstractValidator<ConfiguracaoTextoForm> {

        // Atributos
        private IConfiguracaoTextoBL _IConfiguracaoTextoBL;
        
        // Propriedades
        private IConfiguracaoTextoBL OConfiguracaoTextoBL => _IConfiguracaoTextoBL = _IConfiguracaoTextoBL ?? new ConfiguracaoTextoBL();
        
        //
        public ConfiguracaoTextoFormValidator() {
            
            RuleFor(x => x.ConfiguracaoTextoPadrao.key)
                .NotEmpty().WithMessage("Informe a key do texto");
            
            RuleFor(x => x.ConfiguracaoTextoPadrao.texto)
                .NotEmpty().WithMessage("Informe o texto padrão");
            
            RuleFor(x => x.ConfiguracaoTextoPadrao.key)
                .Must((x, key) => !this.existe(x))
                .WithMessage("Já existe um texto cadastrado com essa key.");
            
        }

        private bool existe(ConfiguracaoTextoForm ViewModel) {
            
            var flagExiste = this.OConfiguracaoTextoBL.query()
                                 .Any(x => x.key.Equals(ViewModel.ConfiguracaoTextoPadrao.key)
                                        && x.id != ViewModel.ConfiguracaoTextoPadrao.id 
                                        && x.idIdioma == null);

            return flagExiste;

        }

    }
    
}