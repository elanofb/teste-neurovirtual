using System.Linq;
using BLL.Services;
using DAL.RedeAfiliados;

namespace BLL.RedeAfiliados.Services {

    public interface IRedeBinariaConsultaBL {

        IQueryable<RedeBinaria> query();

    }

}
