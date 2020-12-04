using FluentValidation.Attributes;
using DAL.Mailings;

namespace WEB.Areas.Mailings.ViewModels {

    [Validator(typeof(TipoMailingFormValidator))]
    public class TipoMailingForm {

        // Propriedades
        public TipoMailing TipoMailing { get; set; }

        public TipoMailingForm() {
            this.TipoMailing = new TipoMailing();
        }
    }
}