using System;

namespace BLL.CuponsDesconto.Services {

    public interface ICupomUtilizacaoValidador {

        UtilRetorno validarUso(int idOrganizacaoParam, string codigoCupom, byte idTipoPagamento);
    }
}