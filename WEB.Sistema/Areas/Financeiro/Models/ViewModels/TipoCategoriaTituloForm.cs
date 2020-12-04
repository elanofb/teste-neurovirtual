using FluentValidation.Attributes;
using DAL.Financeiro;

namespace WEB.Areas.Financeiro.ViewModels{

    [Validator(typeof(TipoCategoriaTituloValidator))]
	public class TipoCategoriaTituloForm{
        public TipoCategoriaTitulo TipoCategoriaTitulo { get; set; } 
	}
}