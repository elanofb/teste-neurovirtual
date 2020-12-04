using System.Collections.Generic;
using System.Linq;
using DAL.AssociadosContribuicoes;

namespace BLL.AssociadosContribuicoes
{
    public interface IAssociadoContribuicaoBoletoBL
    {
        IQueryable<AssociadoContribuicaoBoleto> listar(int idContribuicao);

        /// <summary>
        /// Salvar itens de uma lista de contribuicoes que precisam ser geradas
        /// </summary>
        void salvar(List<AssociadoContribuicaoBoleto> listaFilaContribuicao);
    }
}