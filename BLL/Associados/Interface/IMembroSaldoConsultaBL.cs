using System.Linq;
using DAL.Associados;

namespace BLL.Associados {

    public interface IMembroSaldoConsultaBL {
        /// <summary>
        /// 
        /// </summary>
        IQueryable<MembroSaldo> query(int idMembro, int? idOrganizacaoParam = null);
    }

}