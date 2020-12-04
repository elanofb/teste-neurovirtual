using FluentValidation.Attributes;
using DAL.Publicacoes;

namespace WEB.Areas.Publicacoes.ViewModels {

    [Validator(typeof(ConteudoFormValidation))]
    public class ConteudoForm {
        
        public Conteudo Conteudo { get; set; }
        
    }

}