using System;
using System.Linq;
using System.Data.Entity;
using BLL.Core.Events;
using BLL.Financeiro.Events;
using DAL.Financeiro;
using BLL.Services;
using DAL.Permissao.Security.Extensions;
using DAL.Repository.Base;
using EntityFramework.Extensions;

namespace BLL.Financeiro {

    public class TituloReceitaPagamentoRejeicaoBL : DefaultBL, ITituloReceitaPagamentoRejeicaoBL {

        //Atributos

        //Propriedades

        //
        private EventAggregator eventoPagamentoRecusado = OnPagamentoRecusado.getInstance;

        /// <summary>
        /// Registrar a recusa de pagamento
        /// </summary>
        public UtilRetorno recusarPagamento(int id, string motivo) {

            var OPagamento = this.db.TituloReceitaPagamento
                                    .Include(x => x.TituloReceita)
                                    .Where(x => x.id == id)
                                    .Select(x => new {
                                        x.id,
                                        x.idOrganizacao,
                                        x.idTituloReceita,
                                        x.descricaoParcela,
                                        x.valorDescontoAntecipacao,
                                        x.valorDescontoCupom,
                                        x.valorDesconto,
                                        x.valorOriginal,
                                        x.valorJuros,
                                        x.valorTarifasTransacao,
                                        x.valorTarifasBancarias,
                                        TituloReceita = new {
                                            id = x.idTituloReceita,
                                            x.TituloReceita.idOrganizacao,
                                            x.TituloReceita.nomePessoa,
                                            x.TituloReceita.emailPrincipal,
                                            x.TituloReceita.descricao

                                        }
                                    }).FirstOrDefault().ToJsonObject<TituloReceitaPagamento>();

            if (OPagamento == null) {

                return UtilRetorno.newInstance(true, "Dados do pagamento não localizados.");

            }

            db.TituloReceitaPagamento.Where(x => x.id == id || x.idParcelaPrincipal == id)
                                    .Update(x => new TituloReceitaPagamento {

                                        dtBaixa = DateTime.Now,

                                        flagBaixaAutomatica = true,

                                        idStatusPagamento = StatusPagamentoConst.RECUSADO
                                    });


            this.eventoPagamentoRecusado.subscribe(new OnPagamentoRecusadoHandler());

            this.eventoPagamentoRecusado.publish(OPagamento as object);

            return UtilRetorno.newInstance(false, "");
        }
    }
}
