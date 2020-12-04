using System;
using System.Collections.Generic;
using System.Linq;
using BLL.Financeiro;
using BLL.FinanceiroLancamentos;
using DAL.Financeiro;
using BLL.Services;

namespace BLL.FinanceiroLancamentos {

    public class TituloReceitaCloneFacadeBL : DefaultBL, ITituloReceitaCloneFacadeBL {

        // Atributos Serviços
        private ITituloReceitaBL _TituloReceitaBL;
		
        // Propriedades Serviços
        private ITituloReceitaBL OTituloReceitaBL => _TituloReceitaBL = _TituloReceitaBL ?? new TituloReceitaPadraoBL();
        
        //
        public UtilRetorno clonar(TituloReceita OReceita, int qtdeReplicacoes) {

            var ORetorno = UtilRetorno.newInstance(false);

            var OReceitaClone = this.carregarDadosReceitaBase(OReceita.id);

            if (OReceitaClone == null) {

                ORetorno.flagError = true;
                
                ORetorno.listaErros.Add("A Receita informada não foi encontrada.");

                return ORetorno;
            }

            OReceitaClone.id = 0;

            OReceitaClone.idTituloReceitaOrigem = OReceita.id;

            OReceitaClone.descricao = OReceita.descricao;

            if (OReceita.idPessoa != OReceitaClone.idPessoa) {
                
                OReceitaClone.idPessoa = OReceita.idPessoa;
                
                OReceitaClone.tratarDadosPessoa();
            }

            OReceitaClone.dtCompetencia = OReceita.dtCompetencia;
            
            OReceitaClone.mesCompetencia = Convert.ToByte(OReceita.dtCompetencia?.Month);

            OReceitaClone.anoCompetencia = Convert.ToInt16(OReceita.dtCompetencia?.Year);

            OReceitaClone.dtVencimento = OReceita.dtVencimento;
            
            OReceitaClone.dtVencimentoOriginal = OReceita.dtVencimento;
            
            OReceitaClone.dtQuitacao = null;
            
            ORetorno = this.salvarReceitas(OReceitaClone, qtdeReplicacoes);
            
            return ORetorno;
        }
        
        //
        private TituloReceita carregarDadosReceitaBase(int idTituloReceita) {
            
            var OReceitaBase = this.OTituloReceitaBL.listar(0, 0, 0, "").Where(x => x.id == idTituloReceita)
                                   .Select(x => new {
                                       x.id,
                                       x.idPessoa,
                                       x.idTipoReceita,
                                       x.idReceita,
                                       x.idGatewayPermitido,
                                       x.flagCartaoCreditoPermitido,
                                       x.flagBoletoBancarioPermitido,
                                       x.flagDepositoPermitido,
                                       x.idMacroConta,
                                       x.idCategoria,
                                       x.idCentroCusto,
                                       x.idContaBancaria,
                                       x.idPeriodoRepeticao,
                                       x.limiteParcelamento,
                                       x.descricao,
                                       x.nomePessoa,
                                       x.documentoPessoa,
                                       x.flagEstrangeiro,
                                       x.nroTelPrincipal,
                                       x.nroTelSecundario,
                                       x.emailPrincipal,
                                       x.mesCompetencia,
                                       x.anoCompetencia,
                                       x.qtdeRepeticao,
                                       x.valorTotal,
                                       x.valorJuros,
                                       x.valorDesconto,
                                       x.motivoDesconto,
                                       x.idCupomDesconto,
                                       x.idTabelaImposto,
                                       x.nomeRecibo,
                                       x.documentoRecibo,
                                       x.passaporteRecibo,
                                       x.rneRecibo,
                                       x.logradouroRecibo,
                                       x.numeroRecibo,
                                       x.complementoRecibo,
                                       x.bairroRecibo,
                                       x.cepRecibo,
                                       x.idCidadeRecibo,
                                       x.nomeCidadeRecibo,
                                       x.dtVencimentoOriginal,
                                       x.dtVencimento,
                                       x.dtLimitePagamento,
                                       x.dtQuitacao,
                                       x.dtCompetencia,
                                       x.nroDocumento,
                                       x.nroContrato,
                                       x.observacao,
                                       x.nroNotaFiscal,
                                       x.nroContabil,
                                       x.flagCategoriaPessoa,
                                       x.flagFixa,
                                       listaTituloReceitaPagamento = x.listaTituloReceitaPagamento.Where(c => !c.dtExclusao.HasValue).Select(c => new {
                                           c.descricaoParcela,
                                           c.qtdeParcelas,
                                           c.nroParcela,
                                           c.idParcelaPrincipal,
                                           c.idCentroCusto,
                                           c.idMacroConta,
                                           c.idCategoria,
                                           c.idContaBancaria,
                                           c.valorOriginal,
                                           c.flagGerarRemessa
                                       })
                                   }).FirstOrDefault().ToJsonObject<TituloReceita>();

            return OReceitaBase;
        }

        //
        private UtilRetorno salvarReceitas(TituloReceita OReceitaClone, int qtdeReplicacoes) {
            
            var ORetorno = UtilRetorno.newInstance(false);

            var listaReceitasGeradas = new List<TituloReceita>();

            OReceitaClone.listaTituloReceitaPagamento = this.ajustarPagamentos(OReceitaClone, OReceitaClone.listaTituloReceitaPagamento.ToList());
            
            listaReceitasGeradas.Add(OReceitaClone);

            qtdeReplicacoes--;
            
            for (int i = 1; i <= qtdeReplicacoes; i ++) {

                var OReceitaReplicada = OReceitaClone.ToJsonObject<TituloReceita>(); 
                
                OReceitaReplicada.dtCompetencia = OReceitaClone.dtCompetencia?.AddMonths(i);

                OReceitaReplicada.mesCompetencia = OReceitaReplicada.dtCompetencia?.Month.toByte();
                
                OReceitaReplicada.anoCompetencia = Convert.ToInt16(OReceitaReplicada.dtCompetencia?.Year);
                
                OReceitaReplicada.dtVencimento = OReceitaClone.dtVencimento?.AddMonths(i);
                
                OReceitaReplicada.dtVencimentoOriginal = OReceitaClone.dtVencimento?.AddMonths(i);

                OReceitaReplicada.listaTituloReceitaPagamento = this.ajustarPagamentos(OReceitaReplicada, OReceitaReplicada.listaTituloReceitaPagamento.ToList());
                
                listaReceitasGeradas.Add(OReceitaReplicada);
                
            }
            
            using (var ctx = this.db) {

                ctx.Configuration.AutoDetectChangesEnabled = false;

                ctx.Configuration.ValidateOnSaveEnabled = false;
                
                listaReceitasGeradas.ForEach(x => {

                    x.setDefaultInsertValues();
                    
                    x.listaTituloReceitaPagamento.ForEach(c => {
                    
                        c.setDefaultInsertValues();
                        
                    });

                });

                ctx.TituloReceita.AddRange(listaReceitasGeradas);

                ctx.SaveChanges();

            }

            ORetorno.info = listaReceitasGeradas.FirstOrDefault().id;
            
            return ORetorno;

        }
        
        //
        private List<TituloReceitaPagamento> ajustarPagamentos(TituloReceita OReceitaClone, List<TituloReceitaPagamento> listaPagamentosClone) {

            listaPagamentosClone.ForEach(x => {

                x.idStatusPagamento = StatusPagamentoConst.ABERTO;

                x.dtCompetencia = OReceitaClone.dtCompetencia;

                x.mesCompetencia = OReceitaClone.dtCompetencia?.Month.toByte();

                x.anoCompetencia = Convert.ToInt16(OReceitaClone.dtCompetencia?.Year);

                x.dtVencimentoOriginal = OReceitaClone.dtVencimento;
                
                x.dtVencimento = OReceitaClone.dtVencimento;

            });
            
            return listaPagamentosClone;
        }

    }
    
}