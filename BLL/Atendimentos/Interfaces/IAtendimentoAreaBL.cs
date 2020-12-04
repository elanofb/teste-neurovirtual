using System.Json;
using System.Linq;
using DAL.Atendimentos;

namespace BLL.Atendimentos {

	public interface IAtendimentoAreaBL {

		IQueryable<AtendimentoArea> query(int? idOrganizacaoParam = null);
		
        AtendimentoArea carregar(int id);

        IQueryable<AtendimentoArea> listar(string valorBusca, bool? ativo, int? idOrganizacaoParam = null);

        bool existe(string descricao, int id);

        bool salvar(AtendimentoArea OAtendimentoArea);

        bool excluir(int id);

        JsonMessageStatus alterarStatus(int id);

    }
}