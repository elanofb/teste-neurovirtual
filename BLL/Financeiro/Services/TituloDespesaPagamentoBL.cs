using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using BLL.Services;
using DAL.Financeiro;
using DAL.Permissao.Security.Extensions;
using DAL.Repository.Base;
using EntityFramework.Extensions;

namespace BLL.Financeiro {

    public class TituloDespesaPagamentoBL : DefaultBL, ITituloDespesaPagamentoBL {

        public TituloDespesaPagamento carregar(int id, bool? flagExcluido = false) {

            var query = from TDP in db.TituloDespesaPagamento.Include(x => x.TituloDespesa).Include(x => x.FormaPagamento)
                        where TDP.id == id select TDP;

            query = query.condicoesSeguranca();

            if (flagExcluido == false) {
                query = query.Where(x => x.dtExclusao == null);
            }

            if (flagExcluido == true){
                query = query.Where(x => x.dtExclusao != null);
            }

            return query.FirstOrDefault();
        }

        public IQueryable<TituloDespesaPagamento> listar(int idTituloDespesa, bool? flagExcluido = false) {

            var query = from TDP in db.TituloDespesaPagamento select TDP;

            query = query.condicoesSeguranca();

            if (flagExcluido == false) {
                query = query.Where(x => x.dtExclusao == null);
            }

            if (flagExcluido == true){
                query = query.Where(x => x.dtExclusao != null);
            }

            if (idTituloDespesa > 0) {
                query = query.Where(x => x.idTituloDespesa == idTituloDespesa);
            }

            return query;
        }

        public UtilRetorno registrarPagamento(TituloDespesaPagamento OTituloDespesaPagamento) {

            using(var Context = new DataContext()) {
                try {

                    //Atualizar dados do pagamento
                    var OTituloDespesaPagamentoDB = Context.TituloDespesaPagamento.condicoesSeguranca()
                        .SingleOrDefault(x => x.id == OTituloDespesaPagamento.id && x.dtExclusao == null && x.TituloDespesa.dtQuitacao == null && x.dtPagamento == null);

                    if (OTituloDespesaPagamentoDB == null) {
                        return UtilRetorno.newInstance(true, "Não foi possível localizar o registro para realizar a baixa do pagamento");
                    }

                    int qtdePagamentosAbertos = this.listar(OTituloDespesaPagamentoDB.idTituloDespesa).Count(x => x.dtPagamento == null);

                    OTituloDespesaPagamentoDB.idStatusPagamento = StatusPagamentoConst.PAGO;
                    OTituloDespesaPagamentoDB.dtPagamento = OTituloDespesaPagamento.dtPagamento;
                    OTituloDespesaPagamentoDB.idMeioPagamento = OTituloDespesaPagamento.idMeioPagamento;
                    OTituloDespesaPagamentoDB.idFormaPagamento = OTituloDespesaPagamento.definirFormaPagamento();
                    OTituloDespesaPagamentoDB.dtBaixa = DateTime.Now;
                    OTituloDespesaPagamentoDB.idUsuarioBaixa = User.id();
                    OTituloDespesaPagamentoDB.valorPago = OTituloDespesaPagamento.valorOriginal;
                    OTituloDespesaPagamentoDB.valorOutrasTarifas = OTituloDespesaPagamento.valorOutrasTarifas;
                    OTituloDespesaPagamentoDB.nroBanco = OTituloDespesaPagamento.nroBanco;
                    OTituloDespesaPagamentoDB.nroContrato = OTituloDespesaPagamento.nroContrato;
                    OTituloDespesaPagamentoDB.nroNotaFiscal = OTituloDespesaPagamento.nroNotaFiscal;
                    OTituloDespesaPagamentoDB.nroDocumento = OTituloDespesaPagamento.nroDocumento;
                    OTituloDespesaPagamentoDB.nroAgencia = OTituloDespesaPagamento.nroAgencia;
                    OTituloDespesaPagamentoDB.nroDigitoAgencia = OTituloDespesaPagamento.nroDigitoAgencia;
                    OTituloDespesaPagamentoDB.nroConta = OTituloDespesaPagamento.nroConta;
                    OTituloDespesaPagamentoDB.nroDigitoConta = OTituloDespesaPagamento.nroDigitoConta;
                    OTituloDespesaPagamentoDB.codigoAutorizacao = OTituloDespesaPagamento.codigoAutorizacao;
                    OTituloDespesaPagamentoDB.valorOutrasTarifas = OTituloDespesaPagamento.valorOutrasTarifas;
                    
                    Context.SaveChanges();

                    if(qtdePagamentosAbertos > 1) {
                        return UtilRetorno.newInstance(false, "Baixa do pagamento registrado com sucesso.");
                    }

                    var OTituloDespesa = Context.TituloDespesa.SingleOrDefault(x => x.id == OTituloDespesaPagamentoDB.idTituloDespesa);
                    OTituloDespesa.dtQuitacao = DateTime.Now;
                    Context.SaveChanges();

                    return UtilRetorno.newInstance(false, "Baixa do pagamento registrado com sucesso. Despesa Quitada!");

                } catch(Exception ex) {
                    UtilLog.saveError(ex, $"Erro ao registrar pagamento manualmente {OTituloDespesaPagamento.id}, {OTituloDespesaPagamento.dtPagamento}.");
                }
            }

            return UtilRetorno.newInstance(true, "Não foi possível registrar a baixa pagamento.");
        }

        public UtilRetorno cancelarPagamento(int id) {

            using(var Context = new DataContext()) {
                try {

                    //Atualizar dados do pagamento
                    var OTituloDespesaPagamentoDB = Context.TituloDespesaPagamento.condicoesSeguranca().SingleOrDefault(x => x.id == id && x.TituloDespesa.dtExclusao == null);

                    if (OTituloDespesaPagamentoDB == null) {
                        return UtilRetorno.newInstance(true, "Não foi possível localizar o pagamento para realizar o cancelamento.");
                    }

                    var OTituloDespesaPagamento = OTituloDespesaPagamentoDB.ToJsonObject<TituloDespesaPagamento>(true);
                    
                    OTituloDespesaPagamento.idStatusPagamento = StatusPagamentoConst.ABERTO;
                    OTituloDespesaPagamento.idMeioPagamento = null;
                    OTituloDespesaPagamento.idFormaPagamento = null;
                    OTituloDespesaPagamento.dtPagamento = null;
                    OTituloDespesaPagamento.dtBaixa = null;
                    OTituloDespesaPagamento.idUsuarioBaixa = null;
                    OTituloDespesaPagamento.valorPago = null;
                    OTituloDespesaPagamento.id = 0;
                    OTituloDespesaPagamento.setDefaultInsertValues();
                    Context.TituloDespesaPagamento.Add(OTituloDespesaPagamento);
                    Context.SaveChanges();

                    OTituloDespesaPagamentoDB.motivoExclusao = "Registro excluído pelo processo de cancelamento do pagamento";
                    OTituloDespesaPagamentoDB.dtExclusao = DateTime.Now;
                    OTituloDespesaPagamentoDB.idUsuarioExclusao = User.id();
                    Context.SaveChanges();

                    var OTituloDespesa = Context.TituloDespesa.SingleOrDefault(x => x.id == OTituloDespesaPagamentoDB.idTituloDespesa);
                    OTituloDespesa.dtQuitacao = null;
                    Context.SaveChanges();

                    return UtilRetorno.newInstance(false, "Baixa do pagamento cancelada com sucesso.");

                } catch(Exception ex) {
                    UtilLog.saveError(ex, $"Erro ao cancelar pagamento manualmente {id}.");
                }
            }

            return UtilRetorno.newInstance(true, "Não foi possível realizar o cancelamento da baixa do pagamento.");
        }

        //Remover um registro (exclusão lógica - não remove-se fisicamente)
        public UtilRetorno excluir(int id, string motivo, string flagOutros = "") {
            using(var Context = new DataContext()) {
                try {

                    //Atualizar dados do pagamento
                    var OTituloDespesaPagamentoDB = Context.TituloDespesaPagamento.condicoesSeguranca().SingleOrDefault(x => x.id == id && x.TituloDespesa.dtExclusao == null);

                    if (OTituloDespesaPagamentoDB == null) {
                        return UtilRetorno.newInstance(true, "Não foi possível localizar o registro para realizar a exclusão.");
                    }

                    OTituloDespesaPagamentoDB.motivoExclusao = motivo;
                    OTituloDespesaPagamentoDB.dtExclusao = DateTime.Now;
                    OTituloDespesaPagamentoDB.idUsuarioExclusao = User.id();
                    Context.SaveChanges();

                    if (flagOutros == "NEXT") {
                        Context.TituloDespesaPagamento
                            .Where(x => x.dtExclusao == null && x.idTituloDespesa == OTituloDespesaPagamentoDB.idTituloDespesa && (x.dtVencimento > OTituloDespesaPagamentoDB.dtVencimento || (x.dtVencimento == OTituloDespesaPagamentoDB.dtVencimento && x.id > OTituloDespesaPagamentoDB.id)))
                            .Update(x => new TituloDespesaPagamento() {
                                motivoExclusao = OTituloDespesaPagamentoDB.motivoExclusao,
                                dtExclusao = DateTime.Now,
                                idUsuarioExclusao = OTituloDespesaPagamentoDB.idUsuarioExclusao
                            });
                    }

                    if (flagOutros == "ALL") {
                        db.TituloDespesaPagamento
                            .Where(x => x.dtExclusao == null && x.idTituloDespesa == OTituloDespesaPagamentoDB.idTituloDespesa)
                            .Update(x => new TituloDespesaPagamento() {
                                motivoExclusao = OTituloDespesaPagamentoDB.motivoExclusao,
                                dtExclusao = DateTime.Now,
                                idUsuarioExclusao = OTituloDespesaPagamentoDB.idUsuarioExclusao
                            });
                    }

                    var OTituloDespesa = Context.TituloDespesa.SingleOrDefault(x => x.id == OTituloDespesaPagamentoDB.idTituloDespesa);
                    var pagamentos = this.listar(OTituloDespesaPagamentoDB.idTituloDespesa).Select(x => x.dtPagamento).ToList();
                    var qtdePagamentosAbertos = pagamentos.Count(x => x == null);

                    OTituloDespesa.dtQuitacao = (OTituloDespesa.dtQuitacao == null && qtdePagamentosAbertos == 0 && pagamentos.Any() ? DateTime.Now : (qtdePagamentosAbertos > 0 || !pagamentos.Any() ? null : OTituloDespesa.dtQuitacao));

                    Context.SaveChanges();

                    return UtilRetorno.newInstance(false, "Exclusão do pagamento realizada com sucesso.");

                } catch(Exception ex) {
                    UtilLog.saveError(ex, $"Erro ao cancelar pagamento manualmente {id}.");
                }
            }
            return UtilRetorno.newInstance(true, "Não foi possível realizar a exclusão do pagamento.");
        }

        
        //Concilia o pagamento
        public void conciliarPagamentos(int[] ids, bool flagConciliado) {
            
            if (flagConciliado) {
                db.TituloDespesaPagamento.Where(x => ids.Contains(x.id))
                    .Update(x => new TituloDespesaPagamento { dtConciliacao = DateTime.Now, idUsuarioConciliacao = User.id() });
            }

            if (!flagConciliado) {
                db.TituloDespesaPagamento.Where(x => ids.Contains(x.id))
                    .Update(x => new TituloDespesaPagamento { dtConciliacao = null, idUsuarioConciliacao = null });
            }
            
        }

        public UtilRetorno substituirMacroConta(List<int> ids, int idNovaMacroConta) {

            var idUsuario = User.id();

            if (ids == null) {
                return UtilRetorno.newInstance(true, "Registros não localizado");
            }

            db.TituloDespesaPagamento.Where(x => ids.Contains(x.id))
                .Update(x => new TituloDespesaPagamento { dtAlteracao = DateTime.Now, idUsuarioAlteracao = idUsuario, idMacroConta = idNovaMacroConta });

            return UtilRetorno.newInstance(false);
        }

        public UtilRetorno substituirCategoriaEMacroConta(List<int> ids, int idNovaCategoria, int idNovaMacroConta) {

            var idUsuario = User.id();

            if (ids == null) {
                return UtilRetorno.newInstance(true, "Registros não localizado");
            }

            db.TituloDespesaPagamento.Where(x => ids.Contains(x.id))
                .Update(x => new TituloDespesaPagamento { dtAlteracao = DateTime.Now, idUsuarioAlteracao = idUsuario, idCategoria = idNovaCategoria, idMacroConta = idNovaMacroConta });

            return UtilRetorno.newInstance(false);
        }
    }
}