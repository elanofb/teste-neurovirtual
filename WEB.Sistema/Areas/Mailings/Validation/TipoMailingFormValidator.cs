using BLL.Mailings;
using FluentValidation;

namespace WEB.Areas.Mailings.ViewModels {

    public class TipoMailingFormValidator : AbstractValidator<TipoMailingForm> {

        // Atributos
        private ITipoMailingBL _TipoMailingBL { get; set; }

        // Propriedades
        private ITipoMailingBL OTipoMailingBL => this._TipoMailingBL = this._TipoMailingBL ?? new TipoMailingBL();

        //
        public TipoMailingFormValidator() {

            RuleFor(x => x.TipoMailing.descricao)
                .NotEmpty().WithMessage("A descrição é obrigatória.")
                .Must((x, descricao) => !this.existe(x))
                .WithMessage("Já existe um tipo cadastrado com essa descrição.");
        }

        //
        private bool existe(TipoMailingForm ViewModel) {
            return this.OTipoMailingBL.existe(ViewModel.TipoMailing.descricao, ViewModel.TipoMailing.id);
        }
    }
}