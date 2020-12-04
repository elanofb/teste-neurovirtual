using FluentValidation.Attributes;
using DAL.Cargos;

namespace WEB.Areas.Cargos.ViewModels {

    [Validator(typeof(CargoFormValidator))]
    public class CargoForm {

        public Cargo Cargo { get; set; }

    }

}