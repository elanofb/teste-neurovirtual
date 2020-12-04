using System.Linq;
using System;
using System.Json;
using DAL.Portais;

namespace BLL.Portais{

	public interface IPortalBL{

	    Portal carregar(int id);

	    IQueryable<Portal> listar(string valorBusca, bool? ativo);

	    bool salvar(Portal OPortal);

	    JsonMessageStatus alterarStatus(int id);

	    IQueryable<Portal> existe(string descricao, int id);

        UtilRetorno excluir(int id);
	}
}