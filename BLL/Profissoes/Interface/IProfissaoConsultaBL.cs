using System.Linq;
using DAL.Profissoes;

namespace BLL.Profissoes {

    public interface IProfissaoConsultaBL {

        IQueryable<Profissao> query(int? idOrganizacaoParam = null);

        Profissao carregar(int id);

        IQueryable<Profissao> listar(string valorBusca, bool? ativo);

        bool existe(string descricao, int idDesconsiderado);

    }
}
