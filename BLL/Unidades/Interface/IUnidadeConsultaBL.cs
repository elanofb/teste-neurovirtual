using System.Json;
using System.Linq;
using System.Web;
using DAL.Unidades;
using System.Collections.Generic;

namespace BLL.Unidades {

	public interface IUnidadeConsultaBL{

		IQueryable<Unidade> query(int? idOrganizacaoParam = null);
        bool existe(int idOrganizacaoParam);
		
        
	}
}