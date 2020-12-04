using FluentValidation.Attributes;
using DAL.Publicacoes;

namespace WEB.Areas.Publicacoes.ViewModels {

    [Validator(typeof(ComunicadoFormValidation))]
    public class ComunicadoForm {

        public Noticia Noticia { get; set; }
    }

}