using FluentValidation;

namespace WEB.Areas.Pessoas.ViewModels{

    //
    public class PessoaRelacionamentoFormValidator : AbstractValidator<PessoaRelacionamentoForm> {
        
        //Construtor
        public PessoaRelacionamentoFormValidator() {
            
            RuleFor(x => x.PessoaRelacionamento.idPessoa)
                .NotEmpty().WithMessage("Informe a quem a ocorrência deve estar vinculada.");

            RuleFor(x => x.PessoaRelacionamento.idOcorrenciaRelacionamento)
                .GreaterThan(0).WithMessage("Informe a ocorrência.");

            RuleFor(x => x.PessoaRelacionamento.dtOcorrencia)
                .NotEmpty().WithMessage("Informe a data da ocorrência.");

            //RuleFor(x => x.PessoaRelacionamento.dtOcorrencia)
            //    .LessThanOrEqualTo(DateTime.Today).WithMessage("A data da ocorrência deve ser menor ou igual a hoje.");

			RuleFor(x => x.PessoaRelacionamento.observacao)
                .NotEmpty().WithMessage("Informe as observações da ocorrência.");


        }

    }
}
