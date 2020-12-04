using DAL.Atendimentos;
using FluentValidation.Attributes;

namespace WEB.Areas.Atendimentos.ViewModels{

    [Validator(typeof(AtendimentoAreaFormValidator))]
	public class AtendimentoAreaForm {

        public AtendimentoArea AtendimentoArea { get; set; } 

	}
}