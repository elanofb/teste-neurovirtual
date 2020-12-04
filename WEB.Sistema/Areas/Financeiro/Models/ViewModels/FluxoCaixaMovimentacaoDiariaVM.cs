using System;
using MoreLinq;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using BLL.ContasBancarias;
using BLL.Financeiro;
using DAL.Financeiro;

namespace WEB.Areas.Financeiro.ViewModels {

    public class FluxoCaixaMovimentacaoDiariaVM {

        // Atributos Serviços
        private IContaBancariaBL _ContaBancariaBL;
        private IReceitasDespesasVWBL _ReceitasDespesasVWBL;

        // Propriedades Serviços
        private IContaBancariaBL OContaBancariaBL => _ContaBancariaBL = _ContaBancariaBL ?? new ContaBancariaBL();
        private IReceitasDespesasVWBL OReceitasDespesasVWBL => _ReceitasDespesasVWBL = _ReceitasDespesasVWBL ?? new ReceitasDespesasVWBL();

        // Propriedades
        public decimal saldoInicial { get; set; }

        public List<ReceitaDespesaVW> listaPagamentos { get; set; }

        public List<FluxoCaixaMovimentacaoDiariaDTO> listaMovimentacaoDiaria { get; set; }

        //
        public FluxoCaixaMovimentacaoDiariaVM() {
            this.listaMovimentacaoDiaria = new List<FluxoCaixaMovimentacaoDiariaDTO>();
        }

        //
        public void agruparDatas(FluxoCaixaDiarioForm Form) {

            //Carrega a conta bancaria para pegar o saldo inicial
            var OContaBancaria = this.OContaBancariaBL.carregar(Form.idContaBancaria.toInt());
            
            //Carrega os lançamentos para calcular o saldo da conta até o periodo de inicio da pesquisa
            var query = this.OReceitasDespesasVWBL.listar().Where(x => 
                x.dtPagamento.HasValue &&                
                x.idContaBancaria == Form.idContaBancaria &&
                x.idPagamento > 0 &&
                (x.dtEfetivacao != null && DbFunctions.TruncateTime(x.dtEfetivacao) < Form.dtInicioPeriodo)
            ).ToList();
            
            //Separa entre receita e despesa e calcula o saldo inicial
            var valorTotalDespesa = query.Where(x => x.flagTipoTitulo == "D").Sum(x => x.valor);
            var valorTotalReceita = query.Where(x => x.flagTipoTitulo == "R").Sum(x => x.valorLiquido());

            this.saldoInicial = (OContaBancaria.saldoInicial ?? 0) + (valorTotalReceita - valorTotalDespesa);

            //Carrega a lista de movimentação diaria
            this.listaMovimentacaoDiaria = this.listaPagamentos.Select(x => new FluxoCaixaMovimentacaoDiariaDTO {
                saldoDia = 0,
                saldoAcumulado = 0,
                    
                dtReferencia = x.dtMovimento.Value.Date,
                listaPagamentosMovimentacao = this.listaPagamentos.Where(c => c.dtMovimento.Value.Date == x.dtMovimento.Value.Date).ToList(),
                valorTotalSaida = this.listaPagamentos.Where(c => c.dtMovimento.Value.Date == x.dtMovimento.Value.Date && c.flagTipoTitulo.Equals("D")).Sum(c => c.valor),
                valorTotalEntrada = this.listaPagamentos.Where(c => c.dtMovimento.Value.Date == x.dtMovimento.Value.Date && c.flagTipoTitulo.Equals("R")).Sum(c => c.valorLiquido())
                
            }).DistinctBy(x => x.dtReferencia).ToList();
            
            //Realiza o calculo do saldo acumulado
            var saldoAtual = this.saldoInicial;
            
            foreach (var OMovimentacao in this.listaMovimentacaoDiaria) {

                OMovimentacao.saldoDia = OMovimentacao.valorTotalEntrada - OMovimentacao.valorTotalSaida;

                saldoAtual += OMovimentacao.valorTotalEntrada - OMovimentacao.valorTotalSaida;
                
                OMovimentacao.saldoAcumulado = saldoAtual;
            }
        }
    }
}
