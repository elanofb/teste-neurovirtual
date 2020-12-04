using BLL.Core.Events;
using System;
using System.Linq;
using BLL.LogsAlteracoes;

namespace BLL.Financeiro.Events {

    public class OnAtualizarValorTituloReceitaHandler : IHandler<object> {

        //Atributos
        private TituloReceitaPadraoBL _TituloReceitaPadraoBL;
        private ILogTituloReceitaBL _LogTituloReceitaBL;

        //Propriedades
        private TituloReceitaPadraoBL OTituloReceitaBL => this._TituloReceitaPadraoBL = this._TituloReceitaPadraoBL ?? new TituloReceitaPadraoBL();
        private ILogTituloReceitaBL OLogTituloReceitaBL => _LogTituloReceitaBL = _LogTituloReceitaBL ?? new LogTituloReceitaBL();

        //Chamador das ações do evento
        public void execute(object source) {

            var idTituloReceita = (int)source;

            if (idTituloReceita == 0) {
                throw new Exception("Não foi possível executar o manipulador OnAtualizarValorTituloReceitaHandler pois o objeto é nulo.");
            }

            var OTituloReceita = OTituloReceitaBL.carregar(idTituloReceita);

            if (OTituloReceita == null) {
                throw new Exception("Não foi possível executar o manipulador OnAtualizarValorTituloReceitaHandler pois o objeto é nulo.");
            }

            //Comentado pois gerou problema - Ao excluir uma parcela, zerou o valor do título de receita e por esse motivo o associado não conseguia realizar o pagamento
            //var valorTotal = OTituloReceita.listaTituloReceitaPagamento.Where(x => x.dtExclusao == null).Sum(x => x.valorOriginal).ToString();
            //OLogTituloReceitaBL.alterarCampo(idTituloReceita, "valorTotal", valorTotal, "Alteração realizada pelo evento OnAtualizarValorTituloReceita");
        }
    }
}