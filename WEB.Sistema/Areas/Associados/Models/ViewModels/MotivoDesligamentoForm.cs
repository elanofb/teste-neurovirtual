using FluentValidation.Attributes;
using DAL.Associados;

namespace WEB.Areas.Associados.ViewModels {

    [Validator(typeof(MotivoDesligamentoFormValidator))]
    public class MotivoDesligamentoForm {

        public MotivoDesligamento MotivoDesligamento { get; set; }

    }
}