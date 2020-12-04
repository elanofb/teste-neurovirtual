using FluentValidation.Attributes;
using DAL.Publicacoes;

namespace WEB.Areas.Publicacoes.ViewModels {

    [Validator(typeof(TipoParceiroValidator))]

    public class TipoParceiroForm{
        public TipoParceiro TipoParceiro { get; set; }
    }
}