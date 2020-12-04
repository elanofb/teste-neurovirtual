using DAL.Associados;
using FluentValidation;

namespace WEB.Areas.AssociadosOperacoes.ViewModels{

    public class AssociadoAlterarTipoCadastroFormValidator : AbstractValidator<AssociadoAlterarTipoCadastroForm> {
        
        //Construtor
        public AssociadoAlterarTipoCadastroFormValidator() {
            
            RuleFor(x => x.idTipoCadastro)
                .NotEmpty().WithMessage("O campo Tipo de Cadastro deve ser preenchido.");                                        
            
        }
        

    }
}
