using System.Collections.Generic;
using System.Linq;
using DAL.AssociadosContribuicoes;

namespace BLL.AssociadosContribuicoes {

    public interface IAssociadoContribuicaoFilaBoletoGeracaoBL {

        IQueryable<AssociadoContribuicaoBoletoGeracao> listar(int idContribuicao);

        /// <summary>
        /// Salvar itens de uma lista de boletos de contribuicoes que precisam ser geradas
        /// </summary>
        void salvar(List<AssociadoContribuicaoBoletoGeracao> listaBoletosContribuicao);

        /// <summary>
        /// Salvar itens de uma lista de boletos de contribuicoes que precisam ser geradas
        /// </summary>
        AssociadoContribuicaoBoletoGeracao salvar(AssociadoContribuicaoBoletoGeracao OItemFila);
    }
}