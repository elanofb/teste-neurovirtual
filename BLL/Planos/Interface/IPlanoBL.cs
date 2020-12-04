using System.Linq;
using DAL.Planos;

namespace BLL.Planos {
	public interface IPlanoBL {
        Plano carregar(int id);
        IQueryable<Plano> listar(string valorBusca, string ativo);
        bool salvar(Plano OPlano);
        bool existe(string descricao, int id);
        bool excluir(int[] ids);
	}
}
