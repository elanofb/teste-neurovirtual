using FluentValidation.Attributes;
using DAL.Associados;

namespace WEB.Areas.Associados.ViewModels {

    [Validator(typeof(AssociadoTituloFormValidator))]
    public class AssociadoTituloForm {

        public AssociadoTitulo AssociadoTitulo { get; set; }

        //
        public AssociadoTituloForm() {

        }

    }

}