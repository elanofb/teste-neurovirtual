using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using DAL.Financeiro;

namespace BLL.Financeiro
{
    public interface ITituloReceitaPagamentoConsultaBL{
        
        IQueryable<TituloReceitaPagamento> query(int idOrganizacaoParam);

        TituloReceitaPagamento carregarDadosBaixa(int idOrganizacaoParam, Expression<Func<TituloReceitaPagamento, bool>> condicoes);

        List<TituloReceitaPagamento> carregarDadosParaNFSe(int idOrganizacaoParam, List<int> idTituloReceitaPagamento);
    }
}