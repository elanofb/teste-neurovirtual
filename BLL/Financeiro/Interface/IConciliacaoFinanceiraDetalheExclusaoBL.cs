namespace BLL.Financeiro {

	public interface IConciliacaoFinanceiraDetalheExclusaoBL {
               
        bool excluir(int id);
        bool excluir(int[] id);

    }
}
