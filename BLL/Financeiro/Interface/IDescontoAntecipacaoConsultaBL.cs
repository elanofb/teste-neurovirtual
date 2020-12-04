using System.Linq;
using DAL.Financeiro;
using DAL.Financeiro.Entities;

namespace BLL.Financeiro {

    public interface IDescontoAntecipacaoConsultaBL {

        IQueryable<TituloReceitaDescontoAntecipacao> listar(int idTituloReceita);

        TituloReceitaPagamento carregarDescontoAntecipacao(TituloReceitaPagamento OTituloReceitaPagamento);
    }

}
