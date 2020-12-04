namespace BLL.AssociadosOperacoes {

    public interface IAssociadoSituacaoContribuicaoBL {

        void alterarSituacaoContribuicao(int id, string motivoAlteracao);

        bool atualizarSituacaoContribuicao(int idAssociado, string flagSituacaoContribuicao);

    }

}
