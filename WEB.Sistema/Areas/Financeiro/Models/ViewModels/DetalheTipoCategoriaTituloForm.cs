using FluentValidation.Attributes;
using DAL.Financeiro;

namespace WEB.Areas.Financeiro.ViewModels{

    [Validator(typeof(DetalheTipoCategoriaTituloValidator))]
	public class DetalheTipoCategoriaTituloForm{
        public DetalheTipoCategoriaTitulo DetalheTipoCategoriaTitulo { get; set; } 
	}
}