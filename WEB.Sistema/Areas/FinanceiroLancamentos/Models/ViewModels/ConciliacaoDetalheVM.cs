using System.Collections.Generic;
using System.Linq;
using BLL.Financeiro;
using BLL.Services;
using DAL.Financeiro;

namespace WEB.Areas.FinanceiroLancamentos.ViewModels {

    public class ConciliacaoDetalheVM {

        // Atrbiutos Serviços
        private IConciliacaoFinanceiraConsultaBL _ConciliacaoFinanceiraConsultaBL;

        private IConciliacaoFinanceiraDetalheConsultaBL _IConciliacaoFinanceiraDetalheConsultaBL;
        
        // Propriedades Serviços
        private IConciliacaoFinanceiraConsultaBL OConciliacaoFinanceiraConsultaBL => _ConciliacaoFinanceiraConsultaBL = _ConciliacaoFinanceiraConsultaBL ?? new ConciliacaoFinanceiraConsultaBL();
        
        private IConciliacaoFinanceiraDetalheConsultaBL OConciliacaoFinanceiraDetalheConsultaBL => _IConciliacaoFinanceiraDetalheConsultaBL = _IConciliacaoFinanceiraDetalheConsultaBL ?? new ConciliacaoFinanceiraDetalheConsultaBL();
        
        // Propriedades
        public int idConciliacaoFinanceita { get; set; }

        public ConciliacaoFinanceira OConciliacaoFinanceira { get; set; }
        
        public List<ConciliacaoFinanceiraDetalhe> listaDetalhesConciliacao { get; set; }

        //
        public ConciliacaoDetalheVM() {
            
            this.OConciliacaoFinanceira = new ConciliacaoFinanceira();
            
            this.listaDetalhesConciliacao = new List<ConciliacaoFinanceiraDetalhe>();
        }

        //
        public void carregarInformacoes() {
            
            this.carregarDadosConciliacao();

            if (this.OConciliacaoFinanceira == null) {
                return;
            }

            this.carregarDetalhesConciliacao();
            
        }
        
        //
        private void carregarDadosConciliacao() {
            
            this.OConciliacaoFinanceira = this.OConciliacaoFinanceiraConsultaBL.listar(null, 0)
                                              .Where(x => x.id == this.idConciliacaoFinanceita)
                                              .Select(x => new {
                                                  x.id, 
                                                  x.descricao, 
                                                  x.dtConciliacao
                                              }).FirstOrDefault()
                                                .ToJsonObject<ConciliacaoFinanceira>();
            
        }
        
        //
        private void carregarDetalhesConciliacao() {
            
            this.listaDetalhesConciliacao = this.OConciliacaoFinanceiraDetalheConsultaBL.listar("")
                                                .Where(x => x.idConciliacao == this.OConciliacaoFinanceira.id)
                                                .Select(x => new {
                                                    x.id,
                                                    x.valorConciliado,
                                                    x.idTituloReceitaPagamento,
                                                    TituloReceitaPagamento = new {
                                                        x.TituloReceitaPagamento.dtVencimento,
                                                        x.TituloReceitaPagamento.dtPagamento,
                                                        x.TituloReceitaPagamento.dtPrevisaoCredito,
                                                        TituloReceita = new {
                                                            x.TituloReceitaPagamento.TituloReceita.descricao,
                                                            x.TituloReceitaPagamento.TituloReceita.nomePessoa
                                                        }
                                                    },
                                                    x.idTituloDespesaPagamento,
                                                    TituloDespesaPagamento = new {
                                                        x.TituloDespesaPagamento.dtVencimento,
                                                        x.TituloDespesaPagamento.dtPagamento,
                                                        TituloDespesa = new {
                                                            x.TituloDespesaPagamento.TituloDespesa.descricao,
                                                            x.TituloDespesaPagamento.TituloDespesa.nomePessoaCredor
                                                        },
                                                                            
                                                    },
                                                    UsuarioCadastro = new {
                                                        x.UsuarioCadastro.nome
                                                    }
                                                }).ToListJsonObject<ConciliacaoFinanceiraDetalhe>();
            
        }

    }
}