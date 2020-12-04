using System.Linq;
using DAL.Notificacoes;

namespace BLL.Notificacoes {

    public interface ITemplateMensagemConsultaBL {

        IQueryable<TemplateMensagem> query(int? idOrganizacaoParam = null);

        TemplateMensagem carregar(int id);

        IQueryable<TemplateMensagem> listar(string valorBusca, bool? ativo = true);

    }
    
}