using System.Linq;
using DAL.Pedidos;

namespace BLL.Pedidos {
	public interface IPedidoHistoricoBL {

        IQueryable<PedidoHistorico> listar(int idPedido);

	    PedidoHistorico criarNovaOcorrencia(int idPedido, int idOcorrencia, string obs);

        void criarOcorrenciaPedidoCriado(int idPedido);

	    void criarOcorrenciaFaturamentoPedido(int idPedido);

        void criarOcorrenciaAtendido(int idPedido, string obs);
		
		void criarOcorrenciaEmMontagem(int idPedido, string obs);

	    void criarOcorrenciaPreparado(int idPedido, string obs);
		
		void criarOcorrenciaAguardandoExpedicao(int idPedido, string obs);

        void criarOcorrenciaExpedido(int idPedido, string obs);

        void criarOcorrenciaCancelado(int idPedido, string obs);

        void criarOcorrenciaPago(int idPedido, string obs);

	    void criarOcorrenciaFinalizado(int idPedido, string obs);

        bool salvar(PedidoHistorico OPedidoOcorrencia);
	}
}
