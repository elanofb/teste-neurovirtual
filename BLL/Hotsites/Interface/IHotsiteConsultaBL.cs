using System.Linq;
using DAL.Hotsites;

namespace BLL.Hotsites {

	public interface IHotsiteConsultaBL {

		IQueryable<Hotsite> query(int? idOrganizacaoParam = null);
		
        Hotsite carregar(int id);
		
        IQueryable<Hotsite> listar(string valorBusca, bool? ativo);
		
    }
}
