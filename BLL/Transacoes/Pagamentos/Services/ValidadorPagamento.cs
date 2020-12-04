using System;
using BLL.Transacoes.Movimentos;
using DAL.Associados;
using DAL.Transacoes;
using DAL.Transacoes.Extensions;

namespace BLL.Transacoes.Pagamentos {

    public class ValidadorPagamento : ValidadorBase{
        
        

        /// <summary>
        /// 
        /// </summary>
        public override UtilRetorno validar(MovimentoOperacaoDTO Transacao) {

            var ValidacaoContas = this.validarContas(Transacao);

            if (ValidacaoContas.flagError) {
                
                return ValidacaoContas;
            }
            

            if (Transacao.MembroDestino.idTipoCadastro == (byte)AssociadoTipoCadastroEnum.CONSUMIDOR) {

                return UtilRetorno.newInstance(true, "A conta destino não pertence à um estabelecimento comercial.");
                
            }

            var MovimentoResumo = new MovimentoResumoVW();

            MovimentoResumo.idTipoTransacao = (byte)TipoTransacaoEnum.PAGAMENTO;
            
            MovimentoResumo.descricaoTipoTransacao = "Pagamento";

            MovimentoResumo.captarDados(Transacao);
            
            return UtilRetorno.newInstance(false, "", MovimentoResumo);
        }
    }

}
