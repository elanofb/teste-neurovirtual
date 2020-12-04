using System.Linq;
using DAL.RedeAfiliados;

namespace BLL.RedeAfiliados.Services {

    /// <summary>
    /// 
    /// </summary>
    public interface IRedeEsquerdaConsultaBL {
        
        IQueryable<RedeBinariaEsquerdaVW> query();
    }

}