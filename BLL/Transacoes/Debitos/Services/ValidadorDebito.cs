using System;
using DAL.Associados;
using DAL.Transacoes;
using BLL.Transacoes.Movimentos;
using DAL.Transacoes.Extensions;

namespace BLL.Transacoes.Debitos {

    public class ValidadorDebito : ValidadorBase {

        
        /// <summary>
        /// 
        /// </summary>
        public override UtilRetorno validar(MovimentoOperacaoDTO Transacao) {

            var ValidacaoDestino = this.validarDestino(Transacao);
            
            if (ValidacaoDestino.flagError) {
                
                return ValidacaoDestino;
            }
            
            ValidacaoDestino = this.validarSaldoOrigem(Transacao);
            
            if (ValidacaoDestino.flagError) {
                
                return ValidacaoDestino;
            }
            
            var MovimentoResumo = new MovimentoResumoVW();
            
            MovimentoResumo.idTipoTransacao = Transacao.idTipoTransacao ?? (byte)TipoTransacaoEnum.LANCAMENTO_DEBITO;
            
            MovimentoResumo.descricaoTipoTransacao = "Lançamento de Débito";
            
            MovimentoResumo.captarDados(Transacao);
            
            return UtilRetorno.newInstance(false, "", MovimentoResumo);
        }
    }

}
