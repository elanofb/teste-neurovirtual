using FluentValidation.Attributes;
using DAL.Associados;

namespace WEB.Areas.Associados.ViewModels{

	[Validator(typeof(AssociadoAreaAtuacaoFormValidator))]
	public class AssociadoAreaAtuacaoForm {

		//Propriedades
		public AssociadoAreaAtuacao AssociadoAreaAtuacao { get; set; }

        public int idTipoAssociado { get; set; }

		//Construtor
		public AssociadoAreaAtuacaoForm() { 
		}

	}
}