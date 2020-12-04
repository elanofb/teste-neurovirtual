using FluentValidation.Attributes;
using System.Web;

namespace WEB.Areas.Planos.ViewModels {

    [Validator(typeof(PlanoAnuncioFormValidator))]
    public class PlanoAnuncioForm {

        public Anuncio Anuncio { get; set; }
        public HttpPostedFileBase OArquivo { get; set; }
    }
}