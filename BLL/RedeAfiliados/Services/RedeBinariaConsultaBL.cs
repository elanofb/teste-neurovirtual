using System.Linq;
using BLL.Services;
using DAL.RedeAfiliados;

namespace BLL.RedeAfiliados.Services {

    public class RedeBinariaConsultaBL: DefaultBL, IRedeBinariaConsultaBL {

        /// <summary>
        /// 
        /// </summary>
        public IQueryable<RedeBinaria> query() {

            var query = from Rede in db.RedeBinaria
                        select Rede;

            return query;

        }        
        
    }

}
