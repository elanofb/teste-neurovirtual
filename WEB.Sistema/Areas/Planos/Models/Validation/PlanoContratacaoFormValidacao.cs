using FluentValidation;
using BLL.Planos;

namespace WEB.Areas.Planos.ViewModels{

    //
    public class PlanoContratacaoValidator : AbstractValidator<PlanoContratacaoForm> {
        
		//Atributos
		private IPlanoContratacaoBL _PlanoContratacaoBL; 

		//Propriedades
		private IPlanoContratacaoBL OPlanoContratacaoBL { get{ return (this._PlanoContratacaoBL = this._PlanoContratacaoBL ?? new PlanoContratacaoBL() ); }}

        //Construtor
        public PlanoContratacaoValidator() {
            
            RuleFor(x => x.PlanoContratacao.idPlano)
				.NotEmpty()
				.WithMessage("Informe o Plano a ser contratado.");

            RuleFor(x => x.idContratante)
				.NotEmpty()
				.WithMessage("Informe o Contratante.");

        }
    }
}
