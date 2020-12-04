using System.Linq;
using DAL.Contratos;
using System.Web;

namespace BLL.Contratos {

	public interface IContratoBL {

        Contrato carregar(int id);
		IQueryable<Contrato> listar(string valorBusca, string ativo);
        bool existe(string descricao, int id);
        bool salvar(Contrato OContrato, HttpPostedFileBase OArquivoContrato);
        bool excluir(int[] ids);

	}
}