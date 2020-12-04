using DAL.Ajudas;
using FluentValidation.Attributes;

namespace WEB.Areas.Ajudas.ViewModels {

    //
    [Validator(typeof(AjudaCategoriaFormValidator))]
    public class AjudaCategoriaForm {

        public AjudaCategoria AjudaCategoria { get; set; }

        public AjudaCategoriaForm() {

            AjudaCategoria = new AjudaCategoria();
        }
        
    }
}