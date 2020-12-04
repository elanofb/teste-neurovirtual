using DAL.Associados;

namespace BLL.Email {

	public interface IEnvioAnuidadePaga {

		bool enviar(Associado OAssociado);

	}
}