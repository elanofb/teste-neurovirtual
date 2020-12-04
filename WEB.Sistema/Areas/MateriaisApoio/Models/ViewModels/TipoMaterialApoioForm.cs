using FluentValidation.Attributes;
using DAL.MateriaisApoio;

namespace WEB.Areas.MateriaisApoio.ViewModels{

    [Validator(typeof(TipoMaterialApoioValidator))]
	public class TipoMaterialApoioForm{

		public TipoMaterialApoio TipoMaterialApoio { get; set;} 
	}


}