using System.Linq;
using DAL.Localizacao;
using System.Threading.Tasks;
using DAL.LogsAlteracoes;

namespace BLL.LogsAlteracoes {

	public interface ILogAlteracaoBL {

        LogAlteracao carregar(int id);
        IQueryable<LogAlteracao> listar(int idEntidadeReferencia, int idReferencia, string valorBusca);

	    bool salvar(LogAlteracao OLogAlteracao);

	}
}
