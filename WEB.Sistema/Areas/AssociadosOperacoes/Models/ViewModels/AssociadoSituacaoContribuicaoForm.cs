using FluentValidation.Attributes;

namespace WEB.Areas.AssociadosOperacoes.ViewModels {

    [Validator(typeof(AssociadoSituacaoContribuicaoFormValidator))]
    public class AssociadoSituacaoContribuicaoForm {

        public int id { get; set; }

        public string motivoAlteracao { get; set; }

    }

}