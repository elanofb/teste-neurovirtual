using DAL.Paginas;
using FluentValidation.Attributes;

namespace WEB.Areas.Paginas.ViewModels {

    [Validator(typeof(PaginaEstatutoFormValidator))]
    public class PaginaEstatutoForm {

        public PaginaEstatuto PaginaEstatuto { get; set; }

    }

}