using System.Web;
using DAL.Arquivos;
using FluentValidation.Attributes;

namespace WEB.Areas.Arquivos.ViewModels {

    [Validator(typeof(ArquivoUploadFotoVMValidator))]
    public class ArquivoUploadFotoVM {

        public ArquivoUpload OArquivo { get; set; }

        public string tipoExibicao { get; set;}

        public string urlRedirect { get; set;}

        public HttpPostedFileBase[] FileUpload { get; set; }

    }
    
}