using System;
using System.Collections.Generic;
using BLL.Transacoes.Movimentos;
using DAL.Transacoes;

namespace BLL.Transacoes.Pagamentos {

    public class PagamentoFacade : IPagamentoFacade {
        
        //Atributos
        private IValidadorOperacao             _Validador;
        private IGeradorMovimentoPagamento _GeradorMovimento;
        private IAtualizadorSaldoBL            _AtualizadorSaldoBL;
        
        //Servicos
        private IValidadorOperacao             Validador          => _Validador = _Validador ?? new ValidadorPagamento();
        private IGeradorMovimentoPagamento GeradorMovimento   => _GeradorMovimento = _GeradorMovimento ?? new GeradorMovimentoPagamento();
        private IAtualizadorSaldoBL            AtualizadorSaldoBL => _AtualizadorSaldoBL = _AtualizadorSaldoBL ?? new AtualizadorSaldoBL();

        /// <summary>
        /// 
        /// </summary>
        public UtilRetorno pagar(MovimentoOperacaoDTO Transacao) {

            var RetornoValidacao = this.Validador.validar(Transacao);

            if (RetornoValidacao.flagError) {
                
                return RetornoValidacao;
                
            }

            var ValidacaoSenha = this.Validador.validaSenha(Transacao);
            
            if (ValidacaoSenha.flagError) {

                return ValidacaoSenha;
            }

            var MovimentoResumo = RetornoValidacao.info as MovimentoResumoVW;

            var RetornoMovimento = GeradorMovimento.pagar(MovimentoResumo, Transacao);

            if (RetornoMovimento.flagError) {
                
                return RetornoMovimento;
            }

            var listaMovimentos = RetornoMovimento.info as List<Movimento>;

            var RetornoAtualizador = this.AtualizadorSaldoBL.atualizar(listaMovimentos);
            
            return UtilRetorno.newInstance(false, "Pagamento realizado com sucesso!", listaMovimentos);            
        }
    }

}
