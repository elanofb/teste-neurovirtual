using FluentValidation.Attributes;
using DAL.Publicacoes;

namespace WEB.Areas.Publicacoes.ViewModels {

    [Validator(typeof(VagaEstagioFormValidation))]
    public class VagaEstagioForm {

        public Noticia Noticia { get; set; }
        
    }

}