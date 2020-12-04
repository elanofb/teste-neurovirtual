using BLL.Core.Events;
using System;
using System.Linq;
using BLL.LogsAlteracoes;

namespace BLL.Financeiro.Events {

    public class OnAtualizarValorTituloDespesaHandler : IHandler<object> {

        //Atributos
        private ITituloDespesaBL _ContasAPagarBL;
        private ILogTituloDespesaBL _LogTituloDespesaBL;

        //Propriedades
        private ITituloDespesaBL OTituloDespesaBL => _ContasAPagarBL = _ContasAPagarBL ?? new TituloDespesaPadraoBL();
        private ILogTituloDespesaBL OLogTituloDespesaBL => _LogTituloDespesaBL = _LogTituloDespesaBL ?? new LogTituloDespesaBL();

        //Chamador das ações do evento
        public void execute(object source) {

            var idTituloDespesa = (int)source;

            if (idTituloDespesa == 0) {
                throw new Exception("Não foi possível executar o manipulador OnAtualizarValorTituloDespesaHandler pois o objeto é nulo.");
            }

            var OTituloDespesa = OTituloDespesaBL.carregar(idTituloDespesa);
            if (OTituloDespesa == null) {
                throw new Exception("Não foi possível executar o manipulador OnAtualizarValorTituloDespesaHandler pois o objeto é nulo.");
            }

            var valorTotal = OTituloDespesa.listaTituloDespesaPagamento.Where(x => x.dtExclusao == null).Sum(x => x.valorOriginal).ToString();
            OLogTituloDespesaBL.alterarCampo(idTituloDespesa, "valorTotal", valorTotal, "Alteração realizada pelo evento OnAtualizarValorTituloDespesa");
        }
    }
}