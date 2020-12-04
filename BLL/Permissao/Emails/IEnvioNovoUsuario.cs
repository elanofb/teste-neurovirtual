
using System;
using DAL.Permissao;

namespace BLL.Permissao.Emails {

	public interface IEnvioNovoUsuario {

		UtilRetorno enviar(UsuarioSistema OUsuario, string senhaProvisoria);

	}
}