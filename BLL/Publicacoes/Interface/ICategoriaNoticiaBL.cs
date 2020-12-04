using System.Linq;
using System.Json;
using DAL.Publicacoes;

namespace BLL.Publicacoes {
	public interface ICategoriaNoticiaBL {

		IQueryable<CategoriaNoticia> listar(string valorBusca = "", bool? ativo = null);

		IQueryable<CategoriaNoticia> listarPorTipoNoticia(int idTipoNoticia, bool? ativo = null);

        CategoriaNoticia carregar(int id);

		bool salvar(CategoriaNoticia ONoticia);

        bool existe(string descricao, int id, int idPortal, int idOrganizacao);

        JsonMessageStatus alterarStatus(int id);

		bool excluir(int id);
	}
}
