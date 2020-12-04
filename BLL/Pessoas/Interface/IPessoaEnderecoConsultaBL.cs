using System.Linq;
using DAL.Pessoas;

namespace BLL.Pessoas {

    public interface IPessoaEnderecoConsultaBL {

        /// <summary>
        /// Carregar um endereço a partir do id
        /// </summary>
        PessoaEndereco carregar(int id);
        
        /// <summary>
        /// Carregar um endereço a partir do idPessoa e idTipoEndereco
        /// </summary>
        PessoaEndereco carregar(int idPessoa, int idTipoEndereco);

        /// <summary>
        /// Montagem de query conforme paramentros informados
        /// </summary>
        IQueryable<PessoaEndereco> listar(int idPessoa);
    }
}