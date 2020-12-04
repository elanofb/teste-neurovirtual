using System.Linq;
using DAL.Associados;

namespace BLL.Associados {

    public interface IMembroSaldoVwBL {
        /// <summary>
        /// 
        /// </summary>
        IQueryable<MembroSaldoVW> query(int idMembro);
    }

}