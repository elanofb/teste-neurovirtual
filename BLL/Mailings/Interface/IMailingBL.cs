using System;
using System.Linq;
using DAL.Mailings;

namespace BLL.Mailings {
    public interface IMailingBL {

        Mailing carregar(int id);

        IQueryable<Mailing> listar(string valorBusca, string ativo, int idTipoMailing, int idAssociado, int? idOrganizacaoInf = null);

        bool existe(string email, int idTipoMailing, int idAssociado, int id, int? idOrganizacaoInf = null);

        bool salvar(Mailing OMailing);
        
        UtilRetorno excluir(int id);

    }
}
