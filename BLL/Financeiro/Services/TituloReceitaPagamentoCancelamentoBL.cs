using System;
using System.Linq;
using System.Data.Entity;
using DAL.Financeiro;
using BLL.Services;
using DAL.Permissao.Security.Extensions;
using DAL.Repository.Base;
using EntityFramework.Extensions;

namespace BLL.Financeiro {

    public class TituloReceitaPagamentoCancelamentoBL : DefaultBL, ITituloReceitaPagamentoCancelamentoBL {

        //Atributos

        //Propriedades

        public UtilRetorno cancelarPagamento(int id) {

            using(var Context = new DataContext()) {
                try {

                    //Atualizar dados do pagamento
                    var OTituloReceitaPagamentoDB = Context.TituloReceitaPagamento.condicoesSeguranca().SingleOrDefault(x => x.id == id && x.TituloReceita.dtExclusao == null);

                    if (OTituloReceitaPagamentoDB == null){
                        return UtilRetorno.newInstance(true, "Não foi possível localizar o pagamento para realizar o cancelamento.");
                    }
                    
                    var OTituloReceitaPagamento = OTituloReceitaPagamentoDB.ToJsonObject<TituloReceitaPagamento>(true);

                    OTituloReceitaPagamento.idStatusPagamento = StatusPagamentoConst.ABERTO;
                    OTituloReceitaPagamento.dtPagamento = null;
                    OTituloReceitaPagamento.dtBaixa = null;
                    OTituloReceitaPagamento.idUsuarioBaixa = null;
                    OTituloReceitaPagamento.flagBaixaAutomatica = null;
                    OTituloReceitaPagamento.valorRecebido = null;
                    OTituloReceitaPagamento.idMeioPagamento = null;
                    OTituloReceitaPagamento.idFormaPagamento = null;
                    OTituloReceitaPagamento.id = 0;
                    Context.TituloReceitaPagamento.Add(OTituloReceitaPagamento);
                    Context.SaveChanges();

                    OTituloReceitaPagamentoDB.motivoExclusao = "Registro excluído pelo processo de cancelamento da baixa do pagamento";
                    OTituloReceitaPagamentoDB.dtExclusao = DateTime.Now;
                    OTituloReceitaPagamentoDB.idUsuarioExclusao = User.id();
                    Context.SaveChanges();

                    var OTituloReceita = Context.TituloReceita.SingleOrDefault(x => x.id == OTituloReceitaPagamentoDB.idTituloReceita);
                    OTituloReceita.dtQuitacao = null;
                    Context.SaveChanges();

                    return UtilRetorno.newInstance(false, "Baixa do pagamento cancelada com sucesso.");

                } catch(Exception ex) {
                    UtilLog.saveError(ex, $"Erro ao cancelar pagamento manualmente {id}.");
                }
            }
            return UtilRetorno.newInstance(true, "Não foi possível realizar o cancelamento da baixa do pagamento.");
        }
    }
}
