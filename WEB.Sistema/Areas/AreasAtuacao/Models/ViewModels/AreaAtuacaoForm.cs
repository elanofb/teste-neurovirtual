using FluentValidation.Attributes;
using DAL.AreasAtuacao;

namespace WEB.Areas.AreasAtuacao.ViewModels {

    [Validator(typeof(AreaAtuacaoFormValidator))]
    public class AreaAtuacaoForm {

        public AreaAtuacao AreaAtuacao { get; set; }

    }
}