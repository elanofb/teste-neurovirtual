using System;
using DAL.Associados;

namespace BLL.Email {

	public interface IEnvioSenhaAssociado {

		UtilRetorno enviar(Associado OAssociado, string novaSenha);

	}
}