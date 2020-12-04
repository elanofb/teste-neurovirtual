namespace BLL.Historicos.Interfaces {
    
    public interface IHistoricoAtualizacaoFinalizacaoBL {

        bool finalizarAnalise(int[] ids, bool flagAprovado);

    }
}