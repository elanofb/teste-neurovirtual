using DAL.Atendimentos;

namespace BLL.Atendimentos {

	public interface IAtendimentoAcaoBL {

        void iniciarAtendimento(int idAtendimento);

        void enviarMensagem(AtendimentoHistorico OAtendimentoHistorico);

        void aguardarRetorno(AtendimentoHistorico OAtendimentoHistorico);

        void finalizar(AtendimentoHistorico OAtendimentoHistorico);

        void sair(AtendimentoHistorico OAtendimentoHistorico);

    }
}