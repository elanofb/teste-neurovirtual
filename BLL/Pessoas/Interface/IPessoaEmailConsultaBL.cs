using System.Linq;
using DAL.Pessoas;

namespace BLL.Pessoas {

    public interface IPessoaEmailConsultaBL {

        /// <summary>
        /// Carregar um endereço a partir do id
        /// </summary>
        PessoaEmail carregar(int id);
        
        /// <summary>
        /// Carregar um endereço a partir do idPessoa e idTipoEmail
        /// </summary>
        PessoaEmail carregar(int idPessoa, int idTipoEmail);

        /// <summary>
        /// Montagem de query conforme paramentros informados
        /// </summary>
        IQueryable<PessoaEmail> listar(int idPessoa);
    }
}