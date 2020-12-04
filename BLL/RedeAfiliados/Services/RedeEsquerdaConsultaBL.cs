using System.Linq;
using BLL.Services;
using DAL.RedeAfiliados;

namespace BLL.RedeAfiliados.Services {

    public class RedeEsquerdaConsultaBL: DefaultBL, IRedeEsquerdaConsultaBL {

        /// <summary>
        /// 
        /// </summary>
        public IQueryable<RedeBinariaEsquerdaVW> query() {

            var query = from Rede in db.RedeBinariaEsquerdaVW
                        select Rede;

            return query;

        }        
        
    }

}
