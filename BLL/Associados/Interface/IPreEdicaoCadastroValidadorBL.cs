using System;
using System.Linq;
using DAL.Associados;

namespace BLL.Associados {

	public interface IPreEdicaoCadastroValidadorBL {
		
		/// <summary>
		/// Valida a consistencia dos parametros necessários (criptografados) para acesso não logado a tela de pré edição do cadastro  
		/// </summary>
		///
        UtilRetorno validarParametrosAcesso(string idOrganizacaoBase64, string idOrganizacaoSha512, string idAssociadoBase64, string idAssociadoSha512);

	}
	
}