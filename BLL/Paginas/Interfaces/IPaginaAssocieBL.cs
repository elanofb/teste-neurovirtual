using System;
using System.Data.Entity;
using System.Linq;
using BLL.Services;
using DAL.Paginas;
using DAL.Permissao.Security.Extensions;
using EntityFramework.Extensions;

namespace BLL.Paginas {

	public interface IPaginaAssocieBL {

        PaginaAssocie carregar(int idOrganizacaoParam = 0);
        
		IQueryable<PaginaAssocie> listar(int idOrganizacao);

		bool salvar(PaginaAssocie OPaginaAssocie);

	}
}