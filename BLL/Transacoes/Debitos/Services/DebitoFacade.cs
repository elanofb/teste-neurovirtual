using System;
using System.Collections.Generic;
using System.Linq;
using BLL.Transacoes.Movimentos;
using DAL.Transacoes;

namespace BLL.Transacoes.Debitos {

    public class DebitoFacade : IDebitoFacade {
        
        //Atributos
        private IValidadorOperacao _Validador;
        private IGeradorMovimentoDebito _GeradorMovimento;
        private IAtualizadorSaldoBL _AtualizadorSaldoBL;
        
        //Servicos
        private IValidadorOperacao Validador => _Validador = _Validador ?? new ValidadorDebito();
        private IGeradorMovimentoDebito GeradorMovimento => _GeradorMovimento = _GeradorMovimento ?? new GeradorMovimentoDebito();
        private IAtualizadorSaldoBL AtualizadorSaldoBL => _AtualizadorSaldoBL = _AtualizadorSaldoBL ?? new AtualizadorSaldoBL();

        /// <summary>
        /// 
        /// </summary>
        public UtilRetorno debitar(MovimentoOperacaoDTO Transacao) {
            
            var RetornoValidacao = this.Validador.validar(Transacao);
            
            if (RetornoValidacao.flagError) {
                
                return RetornoValidacao;
                
            }
            
            var MovimentoResumo = RetornoValidacao.info as MovimentoResumoVW;
            
            var RetornoMovimento = GeradorMovimento.debitar(MovimentoResumo);
            
            if (RetornoMovimento.flagError) {
                
                return RetornoMovimento;
            }
            
            var listaMovimentos = RetornoMovimento.info as List<Movimento>;
            
            var RetornoAtualizador = this.AtualizadorSaldoBL.atualizar(listaMovimentos);
            
            return UtilRetorno.newInstance(false, "Lançamento de débito realizado com sucesso!", listaMovimentos.FirstOrDefault());
        }
    }

}
