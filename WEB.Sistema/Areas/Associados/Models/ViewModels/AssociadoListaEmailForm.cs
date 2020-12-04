using DAL.Mailings;
using FluentValidation.Attributes;

namespace WEB.Areas.Associados.ViewModels{

	[Validator(typeof(AssociadoListaEmailFormValidator))]
	public class AssociadoListaEmailForm {

		//Propriedades
		public Mailing Mailing { get; set; }

        public int idTipoAssociado { get; set; }
    }
}