using DAL.Planos;
using FluentValidation.Attributes;

namespace WEB.Areas.Planos.ViewModels {

    [Validator(typeof(PlanoFormValidator))]
    public class PlanoForm {

        public Plano Plano { get; set; }

        //Construtor
        public PlanoForm() {

        }
    }
}