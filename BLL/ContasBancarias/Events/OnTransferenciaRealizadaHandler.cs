using BLL.Core.Events;
using System;
using DAL.ContasBancarias;

namespace BLL.ContasBancarias {

    public class OnTransferenciaRealizadaHandler : IHandler<object> {

        //Propriedades
        private IContaBancariaTransferenciaBL OContaBancariaTransferenciaBL => new ContaBancariaTransferenciaBL();

		//
		public void execute(object source) {

            var OContaMovimentacao = source as ContaBancariaMovimentacao;
            
            this.registrarLancamentos(OContaMovimentacao);

		}
        
        //
        private void registrarLancamentos(ContaBancariaMovimentacao OContaMovimentacao) {

            try {

                this.OContaBancariaTransferenciaBL.registrarLancamentos(OContaMovimentacao);
                
            } catch (Exception ex) {

                UtilLog.saveError(ex, $"Erro ao registrar lançamentos para a movimentação { OContaMovimentacao.id }");

            }

        }
        
	}
}
