using System.Linq;
using DAL.Organizacoes;

namespace BLL.Organizacoes {

	public interface IAcessoRecursoGrupoOrganizacaoBL {

        IQueryable<AcessoRecursoGrupoOrganizacao> listar(int idOrganizacaoParam = 0);

        bool salvar(AcessoRecursoGrupoOrganizacao OAcessoRecursoGrupoOrganizacao);
        
        void excluir(int idOrganizacao, int idRecursoGrupo);

    }
}