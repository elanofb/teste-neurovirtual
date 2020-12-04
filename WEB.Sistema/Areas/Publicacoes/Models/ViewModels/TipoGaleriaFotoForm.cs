using FluentValidation.Attributes;
using DAL.Publicacoes;

namespace WEB.Areas.Publicacoes.ViewModels {

    [Validator(typeof(TipoGaleriaFotoValidator))]

    public class TipoGaleriaFotoForm {
        public TipoGaleriaFoto TipoGaleriaFoto { get; set; }
    }
}