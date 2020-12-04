using System.Linq;
using DAL.Publicacoes;


namespace BLL.Publicacoes {

	public interface IConteudoConsultaBL {
		
        IQueryable<Conteudo> query(int? idOrganizacaoParam = null);
		
		Conteudo carregar(int id);
		
        IQueryable<Conteudo> listar(string valorBusca, bool? ativo = true);

		bool existe(string idInterno, int id, int idOrganizacao);

	}

}
