using System.Linq;
using DAL.Pessoas;

namespace BLL.Pessoas {

    public interface IPessoaTelefoneCadastroBL {

        /// <summary>
        /// Definir se é um insert ou update e enviar o registro para o banco de dados 
        /// </summary>
        bool salvar(PessoaTelefone OPessoaTelefone);
    }
}