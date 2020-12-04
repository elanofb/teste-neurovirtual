using FluentValidation.Attributes;
using DAL.Publicacoes;

namespace WEB.Areas.Publicacoes.ViewModels {

    [Validator(typeof(CategoriaNoticiaValidator))]

    public class CategoriaNoticiaForm {
        public CategoriaNoticia CategoriaNoticia { get; set; }
    }
}