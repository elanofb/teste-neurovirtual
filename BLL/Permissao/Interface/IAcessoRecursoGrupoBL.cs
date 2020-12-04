using System.Linq;
using DAL.Permissao;

namespace BLL.Permissao {

    public interface IAcessoRecursoGrupoBL {

        AcessoRecursoGrupo carregar(int id);

        IQueryable<AcessoRecursoGrupo> listar(string ativo);

        AcessoRecursoGrupo salvar(AcessoRecursoGrupo OAcessoRecursoGrupo);

    }
}
