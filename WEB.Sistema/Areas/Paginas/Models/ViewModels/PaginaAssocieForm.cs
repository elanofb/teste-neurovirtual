using DAL.Paginas;
using FluentValidation.Attributes;

namespace WEB.Areas.Paginas.ViewModels {

    [Validator(typeof(PaginaAssocieFormValidator))]
    public class PaginaAssocieForm {

        public PaginaAssocie PaginaAssocie { get; set; }

    }

}