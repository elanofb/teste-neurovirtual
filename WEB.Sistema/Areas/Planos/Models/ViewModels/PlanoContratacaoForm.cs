using FluentValidation.Attributes;
using DAL.Planos;

namespace WEB.Areas.Planos.ViewModels{

    [Validator(typeof(PlanoContratacaoValidator))]
	public class PlanoContratacaoForm{

		public PlanoContratacao PlanoContratacao { get; set;} 
        public string idContratante { get; set;} 
	}


}