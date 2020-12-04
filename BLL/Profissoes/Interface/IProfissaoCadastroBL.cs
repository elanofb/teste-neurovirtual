using System.Json;
using DAL.Profissoes;

namespace BLL.Profissoes {

    public interface IProfissaoCadastroBL {

        bool salvar(Profissao OProfissao);

        JsonMessageStatus alterarStatus(int id);

    }
}
