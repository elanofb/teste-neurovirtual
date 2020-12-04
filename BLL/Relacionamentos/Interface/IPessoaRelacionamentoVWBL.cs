using System.Linq;
using DAL.Relacionamentos;

namespace BLL.Relacionamentos {

    public interface IPessoaRelacionamentoVWBL {

        IQueryable<PessoaRelacionamentoVW> listar(string valorBusca);

    }
    
}