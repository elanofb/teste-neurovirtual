using DAL.ContasBancarias;
using FluentValidation.Attributes;


namespace WEB.Areas.ContasBancarias.ViewModels {

    [Validator(typeof(ContaBancariaTransferenciaFormValidator))]
    public class ContaBancariaTransferenciaForm {

        public ContaBancariaMovimentacao ContaBancariaMovimentacao { get; set; }

    }

}