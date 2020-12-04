using System;
using System.Collections.Generic;
using BLL.Transacoes.Movimentos;
using DAL.Transacoes;

namespace BLL.Transacoes.Transferencias {

    public class TransferenciaFacade : ITransferenciaFacade {
        
        //Atributos
        private IValidadorOperacao _Validador;
        private IGeradorMovimentoTransferencia _GeradorMovimento;
        private IAtualizadorSaldoBL _AtualizadorSaldoBL;
        
        //Servicos
        private IValidadorOperacao Validador => _Validador = _Validador ?? new ValidadorTransferencia();
        private IGeradorMovimentoTransferencia GeradorMovimento => _GeradorMovimento = _GeradorMovimento ?? new GeradorMovimentoTransferencia();
        private IAtualizadorSaldoBL AtualizadorSaldoBL => _AtualizadorSaldoBL = _AtualizadorSaldoBL ?? new AtualizadorSaldoBL();

        /// <summary>
        /// 
        /// </summary>
        public UtilRetorno transferir(MovimentoOperacaoDTO Transacao) {

            var RetornoValidacao = this.Validador.validar(Transacao);
            
            if (RetornoValidacao.flagError) {
                
                return RetornoValidacao;
                
            }
            
            if (Transacao.flagIgnorarSenha != true) {
            
                var ValidacaoSenha = this.Validador.validaSenha(Transacao);
            
                if (ValidacaoSenha.flagError) {

                    return ValidacaoSenha;
                }
                
            }
            
            var MovimentoResumo = RetornoValidacao.info as MovimentoResumoVW;

            var RetornoMovimento = GeradorMovimento.transferir(MovimentoResumo);

            if (RetornoMovimento.flagError) {
                
                return RetornoMovimento;
            }

            var listaMovimentos = RetornoMovimento.info as List<Movimento>;

            var RetornoAtualizador = this.AtualizadorSaldoBL.atualizar(listaMovimentos);
            
            return UtilRetorno.newInstance(false, "Transferência realizada com sucesso!", listaMovimentos);
        }
    }

}
