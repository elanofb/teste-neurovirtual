using System.Linq;
using BLL.Services;
using DAL.RedeAfiliados;

namespace BLL.RedeAfiliados.Services {

    public class RedeDireitaConsultaBL: DefaultBL, IRedeDireitaConsultaBL {

        //
        public IQueryable<RedeBinariaDireitaVW> query() {

            var query = from Rede in db.RedeBinariaDireitaVW
                        select Rede;

            return query;

        }        
        
    }

}
