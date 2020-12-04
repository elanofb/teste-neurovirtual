using System;
using DAL.Associados;
using DAL.Transacoes;
using BLL.Transacoes.Movimentos;
using DAL.Transacoes.Extensions;

namespace BLL.Transacoes.Transferencias {

    public class ValidadorTransferencia : ValidadorBase {

        
        /// <summary>
        /// 
        /// </summary>
        public override UtilRetorno validar(MovimentoOperacaoDTO Transacao) {

            var ValidacaoContas = this.validarContas(Transacao);

            if (ValidacaoContas.flagError) {
                
                return ValidacaoContas;
            }
            

            if (Transacao.MembroOrigem.idTipoCadastro == (byte)AssociadoTipoCadastroEnum.CONSUMIDOR && Transacao.MembroDestino.idTipoCadastro == (byte)AssociadoTipoCadastroEnum.COMERCIANTE) {

                return UtilRetorno.newInstance(true, "A operação de transferência para estabelecimentos não é permitida.");
                
            }

            var MovimentoResumo = new MovimentoResumoVW();

            MovimentoResumo.idTipoTransacao = (byte)TipoTransacaoEnum.TRANSFERÊNCIA;
            
            MovimentoResumo.descricaoTipoTransacao = "Transferência";

            MovimentoResumo.captarDados(Transacao);
            
            return UtilRetorno.newInstance(false, "", MovimentoResumo);
        }
    }

}
