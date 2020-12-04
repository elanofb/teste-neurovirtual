using System.Linq;
using DAL.Pessoas;

namespace BLL.Pessoas {

    public interface IPessoaTelefoneConsultaBL {

        /// <summary>
        /// Carregar um endereço a partir do id
        /// </summary>
        PessoaTelefone carregar(int id);
        
        /// <summary>
        /// Carregar um endereço a partir do idPessoa e idTipoTelefone
        /// </summary>
        PessoaTelefone carregar(int idPessoa, int idTipoTelefone);

        /// <summary>
        /// Montagem de query conforme paramentros informados
        /// </summary>
        IQueryable<PessoaTelefone> listar(int idPessoa);
    }
}