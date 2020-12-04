using System;
using System.Linq.Expressions;
using DAL.Financeiro;

namespace BLL.Financeiro {

    public interface ITituloDespesaPagamentoCadastroBL {
        
        TituloDespesaPagamento salvar(TituloDespesaPagamento OTituloDespesaPagamento);

        void atualizar(int id, Expression<Func<TituloDespesaPagamento, TituloDespesaPagamento>> updateExpression);

        void atualizar(int[] ids, Expression<Func<TituloDespesaPagamento, TituloDespesaPagamento>> updateExpression);
    }
}