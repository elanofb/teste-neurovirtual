using FluentValidation.Attributes;
using DAL.Associados;

namespace WEB.Areas.Associados.ViewModels {

    [Validator(typeof(MotivoDesativacaoFormValidator))]
    public class MotivoDesativacaoForm {

        public MotivoDesativacao MotivoDesativacao { get; set; }

    }
}