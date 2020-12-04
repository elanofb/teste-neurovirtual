using DAL.MeiosDivulgacao;
using FluentValidation.Attributes;

namespace WEB.Areas.MeiosDivulgacao.ViewModels {

    [Validator(typeof(MeioDivulgacaoFormValidator))]
    public class MeioDivulgacaoForm {

        public MeioDivulgacao MeioDivulgacao { get; set; }

    }

}