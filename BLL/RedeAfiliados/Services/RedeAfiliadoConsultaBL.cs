using System.Linq;
using BLL.Services;
using DAL.RedeAfiliados;

namespace BLL.RedeAfiliados.Services {

    public class RedeAfiliadoConsultaBL: DefaultBL, IRedeAfiliadoConsultaBL {

        /// <summary>
        /// 
        /// </summary>
        public IQueryable<RedeAfiliado> query(int idMembro) {

            var query = from Rede in db.RedeAfiliado
                        where Rede.dtExclusao == null
                        select Rede;

            if (idMembro > 0) {
                query = query.Where(x => x.idMembro == idMembro);
            }

            return query;

        }
        
    }

}
