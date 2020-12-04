using System;
using DAL.Associados;

namespace BLL.AssociadosInstitucional {

	public interface IAssociadoAcessoBL {
		
		Associado login(string login, string senha, int idTipoCadastro, int? idOrganizacaoParam = null);
		
		UtilRetorno recuperarSenha(string login);

		UtilRetorno alterarSenha(int idAssociado, string senhaAtual, string novaSenha, bool flagValidarSenhaAtual = true, int? idOrganizacaoParam = null);

		UtilRetorno alterarSenhaTransacao(int idAssociado, string senhaAtual, string novaSenha, bool flagValidarSenhaAtual = true, int? idOrganizacaoParam = null);
		
        UtilRetorno enviarLinkTrocaSenha(int idAssociado);

	}
}
