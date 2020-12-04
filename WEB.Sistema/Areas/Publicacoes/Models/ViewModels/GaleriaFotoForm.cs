using System.Collections.Generic;
using System.Web;
using FluentValidation.Attributes;
using DAL.Publicacoes;

namespace WEB.Areas.Publicacoes.ViewModels {

    [Validator(typeof(GaleriaFormValidation))]
    public class GaleriaFotoForm {

        public GaleriaFoto GaleriaFoto { get; set; }

        public List<HttpPostedFileBase> OArquivo { get; set; }
        
    }

}