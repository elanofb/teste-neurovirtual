using System.Collections.Generic;
using DAL.Contribuicoes;
using FluentValidation.Attributes;

namespace WEB.Areas.AssociadosNotificacoes.ViewModels {

    [Validator(typeof(AssociadoContribuicaoCobrancaFormValidator))]
    public class AssociadoContribuicaoCobrancaForm {

        public Contribuicao Contribuicao { get; set; }

        public List<int> idsAssociadoContribuicoes { get; set; }

    }
}