using DAL.Profissoes;
using FluentValidation.Attributes;

namespace WEB.Areas.Profissoes.ViewModels {

	[Validator(typeof(ProfissaoValidator))]
	public class ProfissaoForm {

		//Propriedades
		public Profissao OProfissao { get; set; }

        //Construtor
        public ProfissaoForm() { 
		}

	}
}