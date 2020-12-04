using System.Linq;
using DAL.Pessoas;

namespace BLL.Pessoas {

    public interface IPessoaEmailExclusaoBL {

        /// <summary>
        /// Excluir endereço a partir de um id
        /// </summary>
        bool excluir(int idEmailPessoa);

        /// <summary>
        /// Excluir endereço a partir de um idPessoa
        /// </summary>
        bool excluirPorPessoa(int idPessoa);
    }
}