using System.Linq;

namespace BLL.Publicacoes {

	public interface IConteudoExclusaoBL {
               
        bool excluir(int id);
        bool excluir(int[] id);

    }
}
