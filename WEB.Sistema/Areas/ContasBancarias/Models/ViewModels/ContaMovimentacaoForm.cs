using FluentValidation.Attributes;
using DAL.ContasBancarias;

namespace WEB.Areas.ContasBancarias.ViewModels {

    [Validator(typeof(ContaMovimentacaoValidator))]
    public class ContaMovimentacaoForm {

        public string descricaoConta { get; set; }

        public ContaBancariaMovimentacao ContaMovimentacao { get; set; }

        public string urlRetorno { get; set; }
    }
}