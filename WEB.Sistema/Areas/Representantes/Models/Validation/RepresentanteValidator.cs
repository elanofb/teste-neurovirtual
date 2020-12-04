using System;
using System.Linq;
using BLL.Representantes;
using FluentValidation;

namespace WEB.Areas.Representantes.ViewModels{

    //
    public class RepresentanteValidator : AbstractValidator<RepresentanteForm> {
        
        // Atributos
        private IRepresentanteConsultaBL _IRepresentanteConsultaBL;
        
        // Propriedades
        private IRepresentanteConsultaBL ORepresentanteConsultaBL => _IRepresentanteConsultaBL = _IRepresentanteConsultaBL ?? new RepresentanteConsultaBL();
        
        //Construtor
        public RepresentanteValidator() {
            
            RuleFor(x => x.Representante.Pessoa.nome)
                .NotEmpty().WithMessage("Informe o nome.");
            
            When(x => !x.Representante.Pessoa.nroDocumento.isEmpty() && x.Representante.Pessoa.flagTipoPessoa == "F", () => {
                
                RuleFor(x => x.Representante.Pessoa.nroDocumento)
                    .Must((x, nroDocumento) => !this.existeCpf(x)).WithMessage("O CPF informado já foi cadastrado anteriormente.")
                    .Must((x, nroDocumento) => UtilValidation.isCPF(x.Representante.Pessoa.nroDocumento)).WithMessage("Informe um CPF válido.");
                
            });
            
            When(x => !x.Representante.Pessoa.nroDocumento.isEmpty() && x.Representante.Pessoa.flagTipoPessoa == "J", () => {
                
                RuleFor(x => x.Representante.Pessoa.nroDocumento)
                    .Must((x, nroDocumento) => !this.existeCpf(x)).WithMessage("O CNPJ informado já foi cadastrado anteriormente.")
                    .Must((x, nroDocumento) => UtilValidation.isCNPJ(x.Representante.Pessoa.nroDocumento)).WithMessage("Informe um CNPJ válido.");
                
            });
            
        }
        
        //
        private bool existeCpf(RepresentanteForm ViewModel) {

            var documento = UtilString.onlyAlphaNumber(ViewModel.Representante.Pessoa.nroDocumento);
            
            var flagExiste = this.ORepresentanteConsultaBL.query()
                                 .Any(x => x.Pessoa.nroDocumento.Equals(documento) && x.id != ViewModel.Representante.id);

            return flagExiste;

        }
        
    }
}
