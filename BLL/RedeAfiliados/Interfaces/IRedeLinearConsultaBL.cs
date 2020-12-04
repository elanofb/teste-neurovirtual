using System.Linq;
using DAL.RedeAfiliados;

namespace BLL.RedeAfiliados.Services {

    public interface IRedeLinearConsultaBL {
        /// <summary>
        /// 
        /// </summary>
        IQueryable<RedeLinearVW> query(int idMembro);


        IQueryable<RedeLinearVW> listarRedeUnilevel(int idMembro);
    }

}