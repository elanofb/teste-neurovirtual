using System.Json;
using System.Linq;
using DAL.Atendimentos;

namespace BLL.Atendimentos {

	public interface IAtendimentoTipoBL {

		IQueryable<AtendimentoTipo> query(int? idOrganizacaoParam = null);
		
        AtendimentoTipo carregar(int id);

        IQueryable<AtendimentoTipo> listar(string valorBusca, bool? ativo, int? idOrganizacaoParam = null);

        bool existe(string descricao, int id);

        bool salvar(AtendimentoTipo OAtendimentoTipo);

        bool excluir(int id);

        JsonMessageStatus alterarStatus(int id);

    }
}