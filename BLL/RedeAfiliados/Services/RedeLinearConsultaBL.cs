using System.Linq;
using BLL.Services;
using DAL.RedeAfiliados;

namespace BLL.RedeAfiliados.Services {

    public class RedeLinearConsultaBL: DefaultBL, IRedeLinearConsultaBL {

        /// <summary>
        /// 
        /// </summary>
        public IQueryable<RedeLinearVW> query(int idMembro) {

            var query = from Rede in db.RedeLinear
                        select Rede;

            if (idMembro > 0) {
                query = query.Where(x => x.idMembro == idMembro);
            }

            return query;

        }
        

        /// <summary>
        /// 
        /// </summary>
        public IQueryable<RedeLinearVW> listarRedeUnilevel(int idMembro) {
                
            var query = this.query(0);
                 
            query = query.Where(x => x.idIndicador01 == idMembro ||
                                     x.idIndicador02 == idMembro ||
                                     x.idIndicador03 == idMembro ||
                                     x.idIndicador04 == idMembro ||
                                     x.idIndicador05 == idMembro ||
                                     x.idIndicador06 == idMembro ||
                                     x.idIndicador07 == idMembro ||
                                     x.idIndicador08 == idMembro ||
                                     x.idIndicador09 == idMembro ||
                                     x.idIndicador10 == idMembro ||
                                     x.idIndicador11 == idMembro ||
                                     x.idIndicador12 == idMembro ||
                                     x.idIndicador13 == idMembro ||
                                     x.idIndicador14 == idMembro ||
                                     x.idIndicador15 == idMembro
                                     );

            return query;

        }        
        
    }

}
