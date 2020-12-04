
using DAL.Permissao;

namespace BLL.Permissao.Emails {

	public interface IEnvioRecuperacaoSenha {

	    bool enviar(UsuarioSistema OUsuario, string senhaProvisoria);

	}
}