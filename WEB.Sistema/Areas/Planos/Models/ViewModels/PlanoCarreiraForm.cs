using FluentValidation.Attributes;
using DAL.Planos;

namespace WEB.Areas.Planos.ViewModels {
    [Validator(typeof(PlanoCarreiraFormValidator))]
    public class PlanoCarreiraForm {
        public PlanoCarreira PlanoCarreira { get; set; }
        public PlanoCarreiraForm() {
            this.PlanoCarreira = new PlanoCarreira();
        }
    }
}