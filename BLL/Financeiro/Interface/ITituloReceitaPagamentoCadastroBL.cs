using System;
using System.Linq.Expressions;
using DAL.Financeiro;
using DAL.Repository.Base;

namespace BLL.Financeiro {

    public interface ITituloReceitaPagamentoCadastroBL {

        DataContext db { get; }

        TituloReceitaPagamento salvar(TituloReceitaPagamento OTituloReceitaPagamento);
        
        TituloReceitaPagamento inserir(TituloReceitaPagamento OTituloReceitaPagamento);
        
        TituloReceitaPagamento atualizar(TituloReceitaPagamento OTituloReceitaPagamento);

        UtilRetorno atualizarDadosPagamento(TituloReceitaPagamento OPagamento);

        void atualizar(int id, Expression<Func<TituloReceitaPagamento, TituloReceitaPagamento>> updateExpression);

        void atualizar(int[] ids, Expression<Func<TituloReceitaPagamento, TituloReceitaPagamento>> updateExpression);
    }
    

}