using FluentValidation.Attributes;
using DAL.FinanceiroLancamentos;

namespace WEB.Areas.FinanceiroLancamentos.ViewModels {

    [Validator(typeof(DevedorFormValidator))]
    public class DevedorForm {

		public Devedor Devedor { get; set;}

        public string group { get; set; }

		//Construtor
        public DevedorForm() {

            this.Devedor = new Devedor();
        }
    }
}