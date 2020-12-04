using DAL.ContasBancarias;
using FluentValidation.Attributes;


namespace WEB.Areas.ContasBancarias.ViewModels {

    [Validator(typeof(ContaBancariaValidator))]
    public class ContaBancariaForm {
        public ContaBancaria ContaBancaria { get; set; }
    }
}