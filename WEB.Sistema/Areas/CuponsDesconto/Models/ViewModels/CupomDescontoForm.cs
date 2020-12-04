using FluentValidation.Attributes;
using DAL.CuponsDesconto;

namespace WEB.Areas.CuponsDesconto.ViewModels {

    [Validator(typeof(CupomDescontoFormValidator))]
    public class CupomDescontoForm {

        //Propriedades
        public CupomDesconto CupomDesconto { get; set; }

        //Construtor
        public CupomDescontoForm() {
        }
    }
}