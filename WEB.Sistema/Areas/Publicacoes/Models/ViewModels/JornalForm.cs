using System.Web;
using FluentValidation.Attributes;
using DAL.Publicacoes;

namespace WEB.Areas.Publicacoes.ViewModels {

    [Validator(typeof(JornalFormValidation))]
    public class JornalForm {
        
        public Jornal Jornal { get; set; }

        public HttpPostedFileBase[] arrayArquivos { get; set; }
    }

}