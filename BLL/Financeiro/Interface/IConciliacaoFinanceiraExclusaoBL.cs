namespace BLL.Financeiro {

	public interface IConciliacaoFinanceiraExclusaoBL {
               
        bool excluir(int id);
        bool excluir(int[] id);

    }
}
