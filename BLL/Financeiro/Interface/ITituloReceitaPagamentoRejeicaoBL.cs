using System;

namespace BLL.Financeiro
{
    public interface ITituloReceitaPagamentoRejeicaoBL
    {
        /// <summary>
        /// Registrar a recusa de pagamento
        /// </summary>
        UtilRetorno recusarPagamento(int id, string motivo);
    }
}