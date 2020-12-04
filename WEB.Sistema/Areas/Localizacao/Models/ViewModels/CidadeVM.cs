using DAL.Localizacao;
using FluentValidation.Attributes;

namespace WEB.Areas.Localizacao.ViewModels {

    [Validator(typeof(CidadeValidator))]
	public class CidadeVM{

		public Cidade Cidade { get; set;}

	}
    
}