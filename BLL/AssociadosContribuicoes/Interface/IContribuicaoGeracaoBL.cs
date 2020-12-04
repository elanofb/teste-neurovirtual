using System;
using DAL.Associados;
using DAL.AssociadosContribuicoes;
using DAL.Contribuicoes;

namespace BLL.AssociadosContribuicoes {

    public interface IContribuicaoGeracaoBL {

        UtilRetorno gerarCobranca(AssociadoContribuicao OAssociadoContribuicao);

        UtilRetorno gerarCobranca(Associado OAssociado, Contribuicao OContribuicao, DateTime? dtVencimento = null, DateTime? dtNovoVencimento = null, bool flagPrimeiraCobranca = false, decimal valorCustomizado = 0);

        UtilRetorno gerarCobrancaDependente(Associado OAssociado, AssociadoContribuicao OAssociadoContribuicao);

        void verificarIsencao(ref AssociadoContribuicao OAssociadoContribuicao, ContribuicaoPreco Preco, Associado OAssociado);
    }
}