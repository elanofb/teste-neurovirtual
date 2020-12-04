using System.Linq;
using BLL.Relacionamentos;
using FluentValidation;

namespace WEB.Areas.Relacionamentos.ViewModels {
    
    public class OcorrenciaRelacionamentoCadastroFormValidator : AbstractValidator<OcorrenciaRelacionamentoCadastroForm> {

        // Atributos
        private IOcorrenciaRelacionamentoVWConsultaBL _IOcorrenciaRelacionamentoVWConsultaBL;
        
        // Propriedades
        private IOcorrenciaRelacionamentoVWConsultaBL OOcorrenciaRelacionamentoVWConsultaBL => _IOcorrenciaRelacionamentoVWConsultaBL = _IOcorrenciaRelacionamentoVWConsultaBL ?? new OcorrenciaRelacionamentoVWConsultaBL();
        
        //
        public OcorrenciaRelacionamentoCadastroFormValidator() {
            
            RuleFor(x => x.OcorrenciaRelacionamento.descricao)
                .NotEmpty().WithMessage("Informe a descrição do tipo de contato");
            
            RuleFor(x => x.OcorrenciaRelacionamento.descricao)
                .Must((x, descricao) => !this.existe(x))
                .WithMessage("Já existe um tipo de contato cadastrado com essa descrição.");
            
        }

        private bool existe(OcorrenciaRelacionamentoCadastroForm ViewModel) {
            
            var flagExiste = this.OOcorrenciaRelacionamentoVWConsultaBL.listar("", null)
                                 .Any(x => x.descricao.Equals(ViewModel.OcorrenciaRelacionamento.descricao) && x.id != ViewModel.OcorrenciaRelacionamento.id);

            return flagExiste;

        }

    }
    
}