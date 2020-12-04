using FluentValidation;

namespace WEB.Areas.FinanceiroLancamentos.ViewModels {

    public class ReceitaCloneFormValidator : AbstractValidator<ReceitaCloneForm> {

        //
        public ReceitaCloneFormValidator() {

            RuleFor(x => x.TituloReceita.descricao)
                .NotEmpty().WithMessage("Informe o título da despesa.");
            
            RuleFor(x => x.idReferenciaPessoa)
                .NotEmpty().WithMessage("Informe o credor da despesa.");
            
            RuleFor(x => x.TituloReceita.dtCompetencia)
                .NotEmpty().WithMessage("Informe a data da despesa.");
            
            RuleFor(x => x.TituloReceita.dtVencimento)
                .NotEmpty().WithMessage("Informe a data de vencimento da despesa.");
            
            RuleFor(x => x.qtdeReplicacoes)
                .NotEmpty().WithMessage("Informe por quantas vezes deseja replicar despesa.");
            
            When(x => x.qtdeReplicacoes > 0, () => {
                
                RuleFor(x => x.qtdeReplicacoes)
                    .InclusiveBetween(1, 36).WithMessage("A quantidade de replicações da despesa deve estar no intervalor de 1 à 36.");
                
            });
            
        }
        
    }
}
