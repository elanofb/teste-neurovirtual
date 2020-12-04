using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Contratos;

namespace BLL.Contratos {
    public interface IStatusContratoBL {

        IQueryable<StatusContrato> listar(string valorBusca, string ativo);

    }
}
