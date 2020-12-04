using System.Web;
using DAL.Institucionais;
using FluentValidation.Attributes;

namespace WEB.Areas.Institucionais.ViewModels {

    [Validator(typeof(ConvenioFormValidation))]
    public class ConvenioForm {
        
        public Convenio Convenio { get; set; }

        public HttpPostedFileBase OArquivo { get; set; }
    }

}