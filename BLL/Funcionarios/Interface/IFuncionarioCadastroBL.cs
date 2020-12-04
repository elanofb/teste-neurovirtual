using DAL.Funcionarios;
using System.Json;

namespace BLL.Funcionarios {

	public interface IFuncionarioCadastroBL {

        bool salvar(Funcionario OFuncionario);
        JsonMessageStatus alterarStatus(int id);
    }
}
