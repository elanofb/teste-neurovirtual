using System;
using DAL.Associados;


namespace BLL.AssociadosInstitucional.Emails {

	public interface IEnvioLinkSenha {

	    UtilRetorno enviar(Associado OAssociado, string linkRecuperacao = "");

	}
}