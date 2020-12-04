using FluentValidation.Attributes;
using DAL.RamosAtividade;

namespace WEB.Areas.RamosAtividade.ViewModels {

    [Validator(typeof(SetorAtuacaoFormValidator))]
    public class SetorAtuacaoForm {

        public SetorAtuacao SetorAtuacao { get; set; }

    }

}