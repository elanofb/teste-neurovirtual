using DAL.Associados;

namespace BLL.Eventos.Email {

	public interface IEnvioNovoAssociado {

		bool enviar(Associado OAssociado);

	}
}