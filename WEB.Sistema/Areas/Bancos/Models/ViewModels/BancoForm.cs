using DAL.Bancos;
using FluentValidation.Attributes;

namespace WEB.Areas.Bancos.ViewModels{

    [Validator(typeof(BancoFormValidator))]
	public class BancoForm{

        public Banco Banco { get; set; } 

	}
}