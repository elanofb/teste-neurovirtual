using DAL.Transacoes;

namespace BLL.Transacoes.Movimentos {

    public interface ICarregadorDadosOperacao {
        /// <summary>
        /// 
        /// </summary>
        MovimentoOperacaoDTO carregar(MovimentoOperacaoDTO Transacao);
    }

}