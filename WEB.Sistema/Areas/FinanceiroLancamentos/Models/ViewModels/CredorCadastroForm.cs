using FluentValidation.Attributes;
using DAL.FinanceiroLancamentos;

namespace WEB.Areas.FinanceiroLancamentos.ViewModels {

    [Validator(typeof(CredorCadastroFormValidator))]
    public class CredorCadastroForm {

		public Credor Credor { get; set;}

        public string group { get; set; }

		//Construtor
        public CredorCadastroForm() {

            this.Credor = new Credor();
        }
    }
}