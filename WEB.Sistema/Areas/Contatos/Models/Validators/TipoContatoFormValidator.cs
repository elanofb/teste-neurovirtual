using System.Linq;
using BLL.Contatos;
using FluentValidation;

namespace WEB.Areas.Contatos.ViewModels {
    
    public class TipoContatoFormValidator : AbstractValidator<TipoContatoForm> {

        // Atributos
        private ITipoContatoVWConsultaBL _ITipoContatoVWConsultaBL;
        
        // Propriedades
        private ITipoContatoVWConsultaBL OTipoContatoVWConsultaBL => _ITipoContatoVWConsultaBL = _ITipoContatoVWConsultaBL ?? new TipoContatoVWConsultaBL();
        
        //
        public TipoContatoFormValidator() {
            
            RuleFor(x => x.TipoContato.descricao)
                .NotEmpty().WithMessage("Informe a descrição do tipo de contato");
            
            RuleFor(x => x.TipoContato.descricao)
                .Must((x, descricao) => !this.existe(x))
                .WithMessage("Já existe um tipo de contato cadastrado com essa descrição.");
            
        }

        private bool existe(TipoContatoForm ViewModel) {
            
            var flagExiste = this.OTipoContatoVWConsultaBL.listar("", null)
                                 .Any(x => x.descricao.Equals(ViewModel.TipoContato.descricao) && x.id != ViewModel.TipoContato.id);

            return flagExiste;

        }

    }
    
}