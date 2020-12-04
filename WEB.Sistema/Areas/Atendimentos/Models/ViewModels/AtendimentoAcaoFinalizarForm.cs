using DAL.Atendimentos;
using FluentValidation.Attributes;

namespace WEB.Areas.Atendimentos.ViewModels{

    [Validator(typeof(AtendimentoAcaoFinalizarFormValidator))]
	public class AtendimentoAcaoFinalizarForm {

        public AtendimentoHistorico AtendimentoHistorico { get; set; }

        public AtendimentoAcaoFinalizarForm() {

            this.AtendimentoHistorico = new AtendimentoHistorico();

        }

	}
}