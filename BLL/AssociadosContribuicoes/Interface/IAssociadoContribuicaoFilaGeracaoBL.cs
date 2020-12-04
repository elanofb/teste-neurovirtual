using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DAL.AssociadosContribuicoes;

namespace BLL.AssociadosContribuicoes {

    public interface IAssociadoContribuicaoFilaGeracaoBL {

        /// <summary>
        /// Listagem de registros gravados em base de dados conforme parametros informados
        /// </summary>
        IQueryable<AssociadoContribuicaoFilaGeracao> listar(int idContribuicao);

        /// <summary>
        /// Salvar itens de uma lista de contribuicoes que precisam ser geradas
        /// </summary>
        void salvar(List<AssociadoContribuicaoFilaGeracao> listaFilaContribuicao);

        /// <summary>
        /// Salvar itens de uma lista de contribuicoes que precisam ser geradas
        /// </summary>
        AssociadoContribuicaoFilaGeracao salvar(AssociadoContribuicaoFilaGeracao OItemFila);


    }
}