using System.Web;
using FluentValidation.Attributes;
using DAL.Publicacoes;

namespace WEB.Areas.Publicacoes.ViewModels {

    [Validator(typeof(NoticiaFormValidation))]
    public class NoticiaForm {
        
        public Noticia Noticia { get; set; }

        public HttpPostedFileBase OArquivo { get; set; }

        public HttpPostedFileBase OArquivoPDF { get; set; }
    }

}