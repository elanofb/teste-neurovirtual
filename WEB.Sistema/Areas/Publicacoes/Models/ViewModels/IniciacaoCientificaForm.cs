using DAL.Publicacoes;
using FluentValidation.Attributes;

namespace WEB.Areas.Publicacoes.ViewModels {

    [Validator(typeof(IniciacaoCientificaFormValidation))]
    public class IniciacaoCientificaForm {
        
        public Noticia ONoticia { get; set; }

    }

}