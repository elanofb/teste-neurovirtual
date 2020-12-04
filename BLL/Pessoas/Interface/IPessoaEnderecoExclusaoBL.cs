using System.Linq;
using DAL.Pessoas;

namespace BLL.Pessoas {

    public interface IPessoaEnderecoExclusaoBL {

        /// <summary>
        /// Excluir endereço a partir de um id
        /// </summary>
        bool excluir(int idEnderecoPessoa);

        /// <summary>
        /// Excluir endereço a partir de um idPessoa
        /// </summary>
        bool excluirPorPessoa(int idPessoa);
    }
}