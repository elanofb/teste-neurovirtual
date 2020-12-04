using System;
using System.Web;

namespace BLL.Configuracoes {

	public interface IConfiguracaoImagemBL {

		UtilRetorno salvar(HttpPostedFileBase OArquivo, string tipoImagem, int idOrganizacao = 0);
	}
}