using DAL.Institucionais;
using FluentValidation.Attributes;

namespace WEB.Areas.Institucionais.ViewModels {

    [Validator(typeof(AssociacaoHistoriaFormValidation))]
    public class AssociacaoHistoriaForm {

        public AssociacaoHistoria AssociacaoHistoria { get; set; }

    }

}