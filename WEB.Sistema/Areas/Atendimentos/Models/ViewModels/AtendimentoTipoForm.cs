using DAL.Atendimentos;
using FluentValidation.Attributes;

namespace WEB.Areas.Atendimentos.ViewModels{

    [Validator(typeof(AtendimentoTipoFormValidator))]
	public class AtendimentoTipoForm {

        public AtendimentoTipo AtendimentoTipo { get; set; } 

	}
}