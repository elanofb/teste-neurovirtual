using System.Linq;
using DAL.Representantes;


namespace BLL.Representantes {

	public interface IRepresentanteConsultaBL {
		
        IQueryable<Representante> query(int? idOrganizacaoParam = null);
		
		Representante carregar(int id);
		
        IQueryable<Representante> listar(string valorBusca, bool? ativo = true);
		
	}

}
