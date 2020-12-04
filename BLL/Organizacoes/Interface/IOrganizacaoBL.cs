using System.Linq;
using System.Web;
using DAL.Organizacoes;
using System;
using System.Json;

namespace BLL.Organizacoes {

	public interface IOrganizacaoBL {

		Organizacao carregar(int id);

		IQueryable<Organizacao> listar(string valorBusca, bool? ativo, bool flagTodasOrganizacoes = false);

		bool salvar(Organizacao OOrganizacao, HttpPostedFileBase Logotipo);

        JsonMessageStatus alterarStatus(int id);

		UtilRetorno excluir(Organizacao OOrganizacao);
	}
}