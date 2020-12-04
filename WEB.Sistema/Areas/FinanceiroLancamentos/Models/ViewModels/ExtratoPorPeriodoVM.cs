using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using BLL.ContasBancarias;
using BLL.Financeiro;
using BLL.Services;
using DAL.Financeiro;

namespace WEB.Areas.FinanceiroLancamentos.ViewModels {

    public class ExtratoPorPeriodoVM {

        // Atrbiutos Serviços
        private IContaBancariaBL _ContaBancariaBL;
        private IReceitasDespesasVWBL _ReceitasDespesasVWBL;
        
        // Propriedades Serviços
        private IContaBancariaBL OContaBancariaBL => _ContaBancariaBL = _ContaBancariaBL ?? new ContaBancariaBL();
        private IReceitasDespesasVWBL OReceitasDespesasVWBL => _ReceitasDespesasVWBL = _ReceitasDespesasVWBL ?? new ReceitasDespesasVWBL();
        
        // Filtros
        public DateTime? dtVencimentoInicio { get; set; }
        public DateTime? dtVencimentoFim { get; set; }
        
        public int? idContaBancaria { get; set; }
        
        public string flagTipoSaida { get; set; }
        public string valorBusca { get; set; }
        public bool? flagSomentePagos { get; set; }
        public string flagTipoTitulo { get; set; }
        
        public List<int?> idsMacroContas { get; set; }
        public List<int?> idsSubContas { get; set; }
        public List<int?> idsCentrosCusto { get; set; }
        
        // Campos carregados
        public decimal saldoInicialPeriodo { get; set; }
        public decimal saldoParcialPeriodo { get; set; }
        
        public List<ReceitaDespesaVW> listaLancamentos { get; set; }

        //
        public ExtratoPorPeriodoVM() {
            this.listaLancamentos = new List<ReceitaDespesaVW>();
        }

        //
        public void carregarInformacoes() {
            
            if (!this.dtVencimentoInicio.HasValue && !this.dtVencimentoFim.HasValue) {

                this.dtVencimentoInicio = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);

                this.dtVencimentoFim = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.DaysInMonth(DateTime.Today.Year, DateTime.Today.Month));
            }

            if (this.dtVencimentoInicio.HasValue && !this.dtVencimentoFim.HasValue) {
                this.dtVencimentoFim = this.dtVencimentoInicio.Value.AddDays(31);
            }

            if(this.dtVencimentoFim.HasValue && !this.dtVencimentoInicio.HasValue) {
                this.dtVencimentoInicio = this.dtVencimentoFim.Value.AddDays(-31);
            }

            if ((this.dtVencimentoFim.Value - this.dtVencimentoInicio.Value).TotalDays > 31) {
                this.dtVencimentoFim = this.dtVencimentoInicio.Value.AddDays(31);
            }
            
            this.carregarSaldoInicialContas();
            this.carregarSaldoInicialPeriodo();
            this.carregarLancamentos();
        }

        //
        private void carregarSaldoInicialContas() {

            var queryContas = this.OContaBancariaBL.listar("", true);
            
            if (this.idContaBancaria.toInt() == 0) {
                this.saldoInicialPeriodo = queryContas.Select(x => x.saldoInicial > 0 ? x.saldoInicial : 0).Sum().toDecimal();
            }
            
            if (this.idContaBancaria > 0) {
                queryContas = queryContas.Where(x => x.id == this.idContaBancaria);
                
                this.saldoInicialPeriodo = queryContas.Select(x => x.saldoInicial > 0 ? x.saldoInicial : 0).Sum().toDecimal();
            }
        }
        
        //
        private void carregarSaldoInicialPeriodo() {

            var query = this.OReceitasDespesasVWBL.listar().Where(x => 
                x.dtPagamento.HasValue &&   
                x.idPagamento > 0 && 
                ((x.dtEfetivacao == null && DbFunctions.TruncateTime(x.dtPrevisaoEfetivacao) < this.dtVencimentoInicio) || 
                (x.dtEfetivacao != null && DbFunctions.TruncateTime(x.dtEfetivacao) < this.dtVencimentoInicio))
            );

            if (this.idContaBancaria > 0) {
                query = query.Where(x => x.idContaBancaria == this.idContaBancaria);
            }
            
            var listaLancamentosPeriodo = query.ToList().Select(x => new { x.flagTipoTitulo, valor = x.valorLiquido() }).ToList();

            var valorTotalDespesa = listaLancamentosPeriodo.Where(x => x.flagTipoTitulo == "D").Select(x => x.valor).Sum();
            
            var valorTotalReceita = listaLancamentosPeriodo.Where(x => x.flagTipoTitulo == "R").ToList().Select(x => x.valor).Sum();

            this.saldoInicialPeriodo = this.saldoInicialPeriodo + (valorTotalReceita - valorTotalDespesa);

            this.saldoParcialPeriodo = this.saldoInicialPeriodo;
        }

        //
        private void carregarLancamentos() {
            
            var query = this.OReceitasDespesasVWBL.listar().Where(x =>
                (x.dtPagamento == null && DbFunctions.TruncateTime(x.dtVencimento) >= this.dtVencimentoInicio && x.dtVencimento <= this.dtVencimentoFim) || 
                (x.dtPagamento != null && x.dtEfetivacao == null && DbFunctions.TruncateTime(x.dtPrevisaoEfetivacao) >= this.dtVencimentoInicio && DbFunctions.TruncateTime(x.dtPrevisaoEfetivacao) <= this.dtVencimentoFim) || 
                (x.dtPagamento != null && x.dtEfetivacao != null && DbFunctions.TruncateTime(x.dtEfetivacao) >= this.dtVencimentoInicio && DbFunctions.TruncateTime(x.dtEfetivacao) <= this.dtVencimentoFim) 
            );
            
            if (this.idContaBancaria > 0) {
                query = query.Where(x => x.idContaBancaria == this.idContaBancaria);
            }

            if (this.idsMacroContas?.Any() == true) {
                query = query.Where(x => this.idsMacroContas.Contains(x.idMacroConta));
            }
            
            if (this.idsSubContas?.Any() == true) {
                query = query.Where(x => this.idsSubContas.Contains(x.idSubConta));                
            }
            
            if (this.idsCentrosCusto?.Any() == true) {
                query = query.Where(x => this.idsCentrosCusto.Contains(x.idCentroCusto));                
            }
            
            if (this.flagSomentePagos != null) {
                
                if (this.flagSomentePagos == true) {
                    query = query.Where(x => x.dtPagamento.HasValue);    
                }
                
                if (this.flagSomentePagos == false) {
                    query = query.Where(x => !x.dtPagamento.HasValue);
                }
            }

            if (!this.flagTipoTitulo.isEmpty()) {
                query = query.Where(x => x.flagTipoTitulo.Equals(this.flagTipoTitulo));
            }

            if (!this.valorBusca.isEmpty()) {
                query = query.Where(x => x.descricaoTitulo.Contains(this.valorBusca) || x.nomePessoa.Contains(this.valorBusca));
            }
            
            this.listaLancamentos = query.Select(x => new {
                x.idPagamento, 
                x.idTitulo, 
                x.descricaoTitulo, 
                x.dtVencimento, 
                x.dtCadastro, 
                x.nomePessoa,
                x.descricaoMacroConta, 
                x.flagTipoTitulo, 
                x.valor, 
                x.valorRealizado, 
                x.valorTarifasBancarias,
                x.valorTarifasTransacao,
                x.valorOutrasTarifas,
                x.dtPagamento, 
                x.dtEfetivacao, 
                x.dtPrevisaoEfetivacao    
             }).OrderBy(x => x.dtVencimento).ThenBy(x => x.dtCadastro).ToListJsonObject<ReceitaDespesaVW>();

            foreach (var Item in listaLancamentos) {

                Item.descricaoTitulo = $"{Item.nomePessoa} - {Item.descricaoTitulo}";

                Item.dtMovimento = Item.dtEfetivacao.HasValue ? Item.dtEfetivacao : Item.dtPrevisaoEfetivacao;

                if (!Item.dtMovimento.HasValue) {
                    Item.dtMovimento = Item.dtVencimento;
                }
            }

            listaLancamentos = listaLancamentos.OrderBy(x => x.dtMovimento).ToList();
        }   
    }
}