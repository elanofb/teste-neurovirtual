using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Linq.Expressions;
using DAL.Financeiro;
using BLL.Services;

namespace BLL.Financeiro {

    public class TituloReceitaPagamentoConsultaBL : DefaultBL, ITituloReceitaPagamentoConsultaBL {

        //Atributos

        //Propriedades


        //Listagem de opcoes de pagamento realizadas
        public IQueryable<TituloReceitaPagamento> query(int idOrganizacaoParam) {

            var query = from Tit in this.db.TituloReceitaPagamento
                                            .Include(x => x.TituloReceita) select Tit;

            if (idOrganizacaoParam > 0) {
                query = query.Where(x => x.idOrganizacao == idOrganizacaoParam);
            }


            return query;
        }
        
        /// <summary>
        /// Query com campos necessários no processo de baixa de um registro
        /// </summary>
        public TituloReceitaPagamento carregarDadosBaixa(int idOrganizacaoParam, Expression<Func<TituloReceitaPagamento, bool>> condicoes) {

            var baseQuery = from Tit in this.db.TituloReceitaPagamento select Tit;


            if (idOrganizacaoParam > 0) {
                baseQuery = baseQuery.Where(x => x.idOrganizacao == idOrganizacaoParam);
            }
            
            baseQuery = baseQuery.Where(condicoes);

            var ORegistro = baseQuery.Select(x => new {
                                                          x.id,
                                                          x.idTituloReceita,
                                                          
                                                          x.dtPagamento,
                                                          x.dtVencimento,
                                                          x.dtVencimentoOriginal,
                                                          
                                                          x.valorRecebido,
                                                          x.valorOriginal,
                                                          x.valorDesconto,
                                                          x.valorDescontoAntecipacao,
                                                          x.valorDescontoCupom,
                                                          
                                                          x.valorTarifasBancarias,
                                                          x.valorTarifasTransacao,
                                                          x.valorJuros,
                                                          
                                                          x.qtdeParcelas,
                                                          
                                                          x.idMeioPagamento,
                                                          
                                                          x.nroDocumento,
                                                          
                                                          x.descricaoParcela
                                                      })
                                     .FirstOrDefault()
                                     .ToJsonObject<TituloReceitaPagamento>();

            return ORegistro;
        }
        
        /// <summary>
        /// Carregar dados parar preenchimento de NFSe
        /// </summary>
        public List<TituloReceitaPagamento> carregarDadosParaNFSe(int idOrganizacaoParam, List<int> idsTituloReceitaPagamento) {
            
            var listaRegistros = this.query(idOrganizacaoParam)
                .Where(x => idsTituloReceitaPagamento.Contains(x.id))
                .Select(x => new {
                    x.id,
                    x.idTituloReceita,
					    
                    x.valorOriginal, 
                    x.valorRecebido, 
                    x.valorDesconto, 
                    x.valorDescontoAntecipacao, 
                    x.valorDescontoCupom, 
                    x.valorJuros, 
                    x.valorOutrasTarifas, 
                    x.valorTarifasBancarias, 
                    x.valorTarifasTransacao, 
					    
                    x.nomeRecibo,
                    x.documentoRecibo,
                    x.telPrincipal,
                    x.email,
                    
                    cepRecibo = x.cepRecibo == "" ? x.TituloReceita.cepRecibo : x.cepRecibo,
                    complementoRecibo = x.complementoRecibo == "" ? x.TituloReceita.complementoRecibo : x.complementoRecibo,
                    logradouroRecibo = x.logradouroRecibo == "" ? x.TituloReceita.logradouroRecibo : x.logradouroRecibo,
                    numeroRecibo = x.numeroRecibo == "" ? x.TituloReceita.numeroRecibo : x.numeroRecibo,
                    bairroRecibo = x.bairroRecibo == "" ? x.TituloReceita.bairroRecibo : x.bairroRecibo,
                    idCidadeRecibo = !(x.idCidadeRecibo > 0) ? x.TituloReceita.idCidadeRecibo : x.idCidadeRecibo,
                    CidadeRecibo = new {
                        idEstado = x.CidadeRecibo == null ? (x.TituloReceita.CidadeRecibo != null ? x.TituloReceita.CidadeRecibo.idEstado : 0) : x.CidadeRecibo.idEstado
                    },
					    
                    TituloReceita = new {
                        x.TituloReceita.id,
                        x.TituloReceita.valorTotal, 
                        x.TituloReceita.nomePessoa,
                        x.TituloReceita.documentoPessoa,
                        x.TituloReceita.nroTelPrincipal,
                        x.TituloReceita.emailPrincipal,
                        x.TituloReceita.cepRecibo,
                        x.TituloReceita.valorDesconto, 
                        x.TituloReceita.valorJuros,
                        x.TituloReceita.logradouroRecibo,
                        x.TituloReceita.complementoRecibo,
                        x.TituloReceita.numeroRecibo,
                        x.TituloReceita.bairroRecibo,
                        x.TituloReceita.idCidadeRecibo,
                        CidadeRecibo = new {
                            idEstado = x.TituloReceita.CidadeRecibo != null ? x.TituloReceita.CidadeRecibo.idEstado : 0
                        }
                    }
                }).ToListJsonObject<TituloReceitaPagamento>();

            return listaRegistros;
        }

    }
}
