using FluentValidation.Attributes;
using DAL.Associados;

namespace WEB.Areas.Associados.ViewModels{

	[Validator(typeof(AssociadoInstituicaoFormValidator))]
	public class AssociadoInstituicaoForm {

		//Propriedades
		public AssociadoInstituicao AssociadoInstituicao { get; set; }

        public int idTipoAssociado { get; set; }

		//Construtor
		public AssociadoInstituicaoForm() { 
		}

	}
}