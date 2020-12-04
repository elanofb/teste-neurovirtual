using DAL.Arquivos;
using DAL.LinksUteis;
using System.Collections.Generic;
using System.Json;
using System.Linq;
using System.Web;

namespace BLL.LinksUteis {

    public interface ILinkUtilBL {

        IQueryable<LinkUtil> query(int? idOrganizacaoParam = null);

        LinkUtil carregar(int id, bool flagCache = false);

        IQueryable<LinkUtil> listar(string valorBusca, bool? ativo, int? idPortal = 0);

        bool salvar(LinkUtil OVeiculo, HttpPostedFileBase OFoto);

        JsonMessageStatus alterarStatus(int id);

        JsonMessage delete(int[] id);

    }
}