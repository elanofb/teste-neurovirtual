using FluentValidation.Attributes;
using DAL.Associados;

namespace WEB.Areas.Associados.ViewModels{

    [Validator(typeof(CategoriaTipoAssociadoFormValidator))]
	public class CategoriaTipoAssociadoForm {

		public CategoriaTipoAssociado CategoriaTipoAssociado { get; set; }
	}

}