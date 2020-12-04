using DAL.Atendimentos;

namespace WEB.Areas.Atendimentos.ViewModels{
    
	public class AtendimentoAcaoSairForm {

        public AtendimentoHistorico AtendimentoHistorico { get; set; }

        public AtendimentoAcaoSairForm() {

            this.AtendimentoHistorico = new AtendimentoHistorico();

        }

	}
}