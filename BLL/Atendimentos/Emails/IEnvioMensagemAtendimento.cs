using System;
using DAL.Atendimentos;

namespace BLL.Atendimentos {

	public interface IEnvioMensagemAtendimento {

		UtilRetorno enviar(Atendimento OAtendimento, string mensagem);

	}
}