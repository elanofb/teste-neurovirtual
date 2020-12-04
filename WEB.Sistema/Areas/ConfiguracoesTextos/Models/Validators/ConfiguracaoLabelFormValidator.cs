using System.Linq;
using BLL.ConfiguracoesTextos;
using FluentValidation;

namespace WEB.Areas.ConfiguracoesTextos.ViewModels {
    
    public class ConfiguracaoLabelFormValidator : AbstractValidator<ConfiguracaoLabelForm> {

        // Atributos
        private IConfiguracaoLabelBL _IConfiguracaoLabelBL;
        
        // Propriedades
        private IConfiguracaoLabelBL OConfiguracaoLabelBL => _IConfiguracaoLabelBL = _IConfiguracaoLabelBL ?? new ConfiguracaoLabelBL();
        
        //
        public ConfiguracaoLabelFormValidator() {
            
            RuleFor(x => x.ConfiguracaoLabelPadrao.key)
                .NotEmpty().WithMessage("Informe a key da label");
            
            RuleFor(x => x.ConfiguracaoLabelPadrao.label)
                .NotEmpty().WithMessage("Informe a label padrão");
            
            RuleFor(x => x.ConfiguracaoLabelPadrao.key)
                .Must((x, key) => !this.existe(x))
                .WithMessage("Já existe uma label cadastrada com essa key.");
            
        }

        private bool existe(ConfiguracaoLabelForm ViewModel) {
            
            var flagExiste = this.OConfiguracaoLabelBL.query()
                                 .Any(x => x.key.Equals(ViewModel.ConfiguracaoLabelPadrao.key)
                                        && x.id != ViewModel.ConfiguracaoLabelPadrao.id 
                                        && x.idIdioma == null);

            return flagExiste;

        }

    }
    
}