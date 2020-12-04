using System.Linq;
using DAL.Atendimentos;

namespace BLL.Atendimentos {

	public interface IAtendimentoConsultaBL {

        Atendimento carregar(int id, int? idOrganizacaoParam = null);

        IQueryable<Atendimento> listar(bool? ativo, int? idOrganizacaoParam = null);

    }

}