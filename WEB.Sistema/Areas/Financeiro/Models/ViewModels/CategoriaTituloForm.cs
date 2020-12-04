using FluentValidation.Attributes;
using DAL.Financeiro;

namespace WEB.Areas.Financeiro.ViewModels{

    [Validator(typeof(CategoriaTituloValidator))]
	public class CategoriaTituloForm{
        public CategoriaTitulo CategoriaTitulo { get; set; } 

        ///Propriedade para auxiliar os modais de cadastro
        public string group { get; set; }
	}
}