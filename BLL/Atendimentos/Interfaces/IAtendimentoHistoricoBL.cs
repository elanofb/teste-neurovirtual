using System.Json;
using System.Linq;
using DAL.Atendimentos;

namespace BLL.Atendimentos {

	public interface IAtendimentoHistoricoBL {

		IQueryable<AtendimentoHistorico> query(int? idOrganizacaoParam = null);
		
        AtendimentoHistorico carregar(int id);

        IQueryable<AtendimentoHistorico> listar(int idAtendimento);

        bool salvar(AtendimentoHistorico OAtendimentoHistorico);

        bool excluir(int id);

        JsonMessageStatus alterarStatus(int id);
    }
}