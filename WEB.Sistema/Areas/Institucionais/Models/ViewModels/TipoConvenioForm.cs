using FluentValidation.Attributes;
using DAL.Institucionais;

namespace WEB.Areas.Institucionais.ViewModels {

    [Validator(typeof(TipoConvenioValidator))]
    public class TipoConvenioForm {

        public TipoConvenio TipoConvenio { get; set; }

        public TipoConvenioForm() {

        }

    }
}