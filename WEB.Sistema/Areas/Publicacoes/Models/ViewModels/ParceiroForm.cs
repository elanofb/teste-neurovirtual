using FluentValidation.Attributes;
using DAL.Publicacoes;
using System.Web;

namespace WEB.Areas.Publicacoes.ViewModels {

    [Validator(typeof(ParceiroFormValidator))]
    public class ParceiroForm {

        public Parceiro Parceiro { get; set; }
        public HttpPostedFileBase OArquivo { get; set; }
    }
}