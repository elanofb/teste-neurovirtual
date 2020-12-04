using DAL.Associados;
using DAL.Transacoes;

namespace BLL.Transacoes.Compras {

    public interface IMediadorBase {
        /// <summary>
        /// 
        /// </summary>
        MovimentoOperacaoDTO carregarDados(int idPessoaOrigem, int idPessoaDestino, decimal valorTransacao, int idReferencia);
        
        /// <summary>
        /// 
        /// </summary>
        Associado carregarMembroOrigem(int idPessoaOrigem);

        /// <summary>
        /// 
        /// </summary>
        Associado carregarMembroDestino(int idPessoaDestino);
    }

}