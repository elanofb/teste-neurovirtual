using System.Linq;
using DAL.RedeAfiliados;

namespace BLL.RedeAfiliados.Services {

    public interface IRedeDireitaConsultaBL {
        
        IQueryable<RedeBinariaDireitaVW> query();
    }

}