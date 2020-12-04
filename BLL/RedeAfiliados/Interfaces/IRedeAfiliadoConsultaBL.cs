using System.Linq;
using DAL.RedeAfiliados;

namespace BLL.RedeAfiliados.Services {

    public interface IRedeAfiliadoConsultaBL {
        /// <summary>
        /// 
        /// </summary>
        IQueryable<RedeAfiliado> query(int idMembro);
    }

}