using FluentValidation.Attributes;
using DAL.RamosAtividade;

namespace WEB.Areas.RamosAtividade.ViewModels {

    [Validator(typeof(RamoAtividadeFormValidator))]
    public class RamoAtividadeForm {

        public RamoAtividade RamoAtividade { get; set; }

    }

}