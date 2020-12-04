using DAL.Atendimentos;
using FluentValidation.Attributes;

namespace WEB.Areas.Atendimentos.ViewModels{

    [Validator(typeof(AtendimentoAcaoMensagemFormValidator))]
	public class AtendimentoAcaoMensagemForm {

        public AtendimentoHistorico AtendimentoHistorico { get; set; }

        public AtendimentoAcaoMensagemForm() {

            this.AtendimentoHistorico = new AtendimentoHistorico();

        }

	}
}