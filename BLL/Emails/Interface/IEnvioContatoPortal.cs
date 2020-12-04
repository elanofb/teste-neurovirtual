using System;
using DAL.FaleConosco;

namespace BLL.Email {

	public interface IEnvioContatoPortal {

		UtilRetorno enviar(ContatoPortal OContato);

	}
}