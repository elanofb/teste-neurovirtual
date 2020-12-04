using System;
using System.Collections.Generic;
using System.Linq;
using BLL.Financeiro;
using DAL.Financeiro;
using BLL.Services;

namespace BLL.FinanceiroLancamentos {

    public class TituloDespesaCloneFacadeBL : DefaultBL, ITituloDespesaCloneFacadeBL {

        // Atributos Serviços
        private ITituloDespesaBL _TituloDespesaBL;
		
        // Propriedades Serviços
        private ITituloDespesaBL OTituloDespesaBL => _TituloDespesaBL = _TituloDespesaBL ?? new TituloDespesaPadraoBL();
        
        //
        public UtilRetorno clonar(TituloDespesa ODespesa, int qtdeReplicacoes) {

            var ORetorno = UtilRetorno.newInstance(false);

            var ODespesaClone = this.carregarDadosDespesaBase(ODespesa.id);

            if (ODespesaClone == null) {

                ORetorno.flagError = true;
                
                ORetorno.listaErros.Add("A despesa informada não foi encontrada.");

                return ORetorno;
            }

            ODespesaClone.id = 0;

            ODespesaClone.idTituloDespesaOrigem = ODespesa.id;

            ODespesaClone.descricao = ODespesa.descricao;

            ODespesaClone.idPessoa = ODespesa.idPessoa;

            ODespesaClone.dtDespesa = ODespesa.dtDespesa;
            
            ODespesaClone.mesCompetencia = Convert.ToByte(ODespesa.dtDespesa?.Month);

            ODespesaClone.anoCompetencia = Convert.ToInt16(ODespesa.dtDespesa?.Year);

            ODespesaClone.dtVencimento = ODespesa.dtVencimento;
            
            ODespesaClone.dtQuitacao = null;
            
            ORetorno = this.salvarDespesas(ODespesaClone, qtdeReplicacoes);
            
            return ORetorno;
        }
        
        //
        private TituloDespesa carregarDadosDespesaBase(int idTituloDespesa) {
            
            var ODespesaBase = this.OTituloDespesaBL.listar("").Where(x => x.id == idTituloDespesa)
                                   .Select(x => new {
                                       x.id,
                                       x.descricao,
                                       x.idTipoDespesa,
                                       x.idDespesa,
                                       x.idPessoa,
                                       x.flagCategoriaPessoa,
                                       x.idCentroCusto,
                                       x.idMacroConta,
                                       x.idCategoria,
                                       x.idContaBancaria,
                                       x.idPeriodoRepeticao,
                                       x.qtdeRepeticao,
                                       x.idAgrupador,
                                       x.nroDocumento,
                                       x.nomePessoaCredor,
                                       x.documentoPessoaCredor,
                                       x.nroTelPrincipalCredor,
                                       x.nroTelSecundarioCredor,
                                       x.emailPrincipalCredor,
                                       x.anoCompetencia,
                                       x.mesCompetencia,
                                       x.dtDespesa,
                                       x.dtVencimento,
                                       x.dtQuitacao,
                                       x.valorTotal,
                                       x.valorJuros,
                                       x.observacao,
                                       x.flagFixa,
                                       x.nroNotaFiscal,
                                       x.nroContabil,
                                       x.nroContrato,
                                       listaTituloDespesaPagamento = x.listaTituloDespesaPagamento.Where(c => !c.dtExclusao.HasValue).Select(c => new {
                                           c.descParcela,
                                           c.idCentroCusto,
                                           c.idMacroConta,
                                           c.idCategoria,
                                           c.idContaBancaria,
                                           c.valorOriginal
                                       })
                                   }).FirstOrDefault().ToJsonObject<TituloDespesa>();

            return ODespesaBase;
        }

        //
        private UtilRetorno salvarDespesas(TituloDespesa ODespesaClone, int qtdeReplicacoes) {
            
            var ORetorno = UtilRetorno.newInstance(false);

            var listaDespesasGeradas = new List<TituloDespesa>();

            ODespesaClone.listaTituloDespesaPagamento = this.ajustarPagamentos(ODespesaClone, ODespesaClone.listaTituloDespesaPagamento.ToList());
            
            listaDespesasGeradas.Add(ODespesaClone);

            qtdeReplicacoes--;
            
            for (int i = 1; i <= qtdeReplicacoes; i ++) {

                var ODespesaReplicada = ODespesaClone.ToJsonObject<TituloDespesa>(); 
                
                ODespesaReplicada.dtDespesa = ODespesaClone.dtDespesa?.AddMonths(i);

                ODespesaReplicada.mesCompetencia = ODespesaReplicada.dtDespesa?.Month.toByte();
                
                ODespesaReplicada.anoCompetencia = Convert.ToInt16(ODespesaReplicada.dtDespesa?.Year);
                
                ODespesaReplicada.dtVencimento = ODespesaClone.dtVencimento?.AddMonths(i);

                ODespesaReplicada.listaTituloDespesaPagamento = this.ajustarPagamentos(ODespesaReplicada, ODespesaReplicada.listaTituloDespesaPagamento.ToList());
                
                listaDespesasGeradas.Add(ODespesaReplicada);
                
            }
            
            using (var ctx = this.db) {

                ctx.Configuration.AutoDetectChangesEnabled = false;

                ctx.Configuration.ValidateOnSaveEnabled = false;
                
                listaDespesasGeradas.ForEach(x => {

                    x.setDefaultInsertValues();
                    
                    x.listaTituloDespesaPagamento.ForEach(c => {
                    
                        c.setDefaultInsertValues();
                        
                    });

                });

                ctx.TituloDespesa.AddRange(listaDespesasGeradas);

                ctx.SaveChanges();

            }

            ORetorno.info = listaDespesasGeradas.FirstOrDefault().id;
            
            return ORetorno;

        }
        
        //
        private List<TituloDespesaPagamento> ajustarPagamentos(TituloDespesa ODespesaClone, List<TituloDespesaPagamento> listaPagamentosClone) {

            listaPagamentosClone.ForEach(x => {

                x.idStatusPagamento = StatusPagamentoConst.ABERTO;

                x.dtCompetencia = ODespesaClone.dtDespesa;

                x.mesCompetencia = ODespesaClone.dtDespesa?.Month.toByte();

                x.anoCompetencia = Convert.ToInt16(ODespesaClone.dtDespesa?.Year);

                x.dtVencimento = ODespesaClone.dtVencimento;

            });
            
            return listaPagamentosClone;
        }

    }
    
}