using System.Linq;
using DAL.Localizacao;

namespace BLL.Localizacao {
    public interface ITipoRegiaoBL {
        /// <summary>
        /// Carregar registro a partir do ID
        /// </summary>
        TipoRegiao carregar(int id);

        /// <summary>
        /// Listagem de registros de acordo com parametros informados
        /// </summary>
        IQueryable<TipoRegiao> listar(string valorBusca, string ativo);
    }
}