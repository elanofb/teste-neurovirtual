using FluentValidation.Attributes;
using DAL.Associados;

namespace WEB.Areas.Associados.ViewModels{

    [Validator(typeof(TipoAssociadoFormValidator))]
	public class TipoAssociadoForm{

		public TipoAssociado TipoAssociado { get; set; }
	}

}