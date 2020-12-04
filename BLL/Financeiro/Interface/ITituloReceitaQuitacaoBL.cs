using DAL.Financeiro;

namespace BLL.Financeiro
{
    public interface ITituloReceitaQuitacaoBL
    {
        TituloReceita liquidar(TituloReceita OTituloReceita);
    }
}