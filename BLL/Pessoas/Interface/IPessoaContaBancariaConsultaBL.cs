using System.Linq;
using DAL.Pessoas;

namespace BLL.Pessoas {

    public interface IPessoaContaBancariaConsultaBL {
        
        IQueryable<PessoaContaBancaria> query(int? idOrganizacaoParam = null);

        /// <summary>
        /// Carregar um endereço a partir do id
        /// </summary>
        PessoaContaBancaria carregar(int id);

        /// <summary>
        /// Montagem de query conforme paramentros informados
        /// </summary>
        IQueryable<PessoaContaBancaria> listar(int idPessoa);
        
    }
}