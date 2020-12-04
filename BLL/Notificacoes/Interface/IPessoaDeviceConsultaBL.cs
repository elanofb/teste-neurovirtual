using System.Linq;
using DAL.Notificacoes;

namespace BLL.Notificacoes {

    public interface IPessoaDeviceConsultaBL {

        IQueryable<PessoaDevice> query(int? idOrganizacaoParam = null);

        PessoaDevice carregar(int id);

        IQueryable<PessoaDevice> listar(string valorBusca, bool? ativo = true);

    }
    
}