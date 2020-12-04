using System;
using System.Json;
using System.Linq;
using DAL.Publicacoes;
using System.Web;

namespace BLL.Publicacoes {

	public interface IPublicacaoBL {
		
	    IQueryable<Noticia> query(int? idOrganizacaoParam = null);

		Noticia carregar(int id);

		IQueryable<Noticia> listar(string valorBusca, string ativo = "S", int? idPortal = 0);
		
		bool salvar(Noticia OPublicacao, HttpPostedFileBase OArquivo);

		bool existe(Noticia OPublicacao);

        JsonMessageStatus alterarStatus(int id);

		UtilRetorno excluir(int id);

	}
}
