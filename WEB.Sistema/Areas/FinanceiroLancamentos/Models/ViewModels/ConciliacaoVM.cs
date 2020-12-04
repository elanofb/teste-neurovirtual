using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using BLL.Financeiro;
using BLL.Services;
using DAL.Financeiro;

namespace WEB.Areas.FinanceiroLancamentos.ViewModels {

    public class ConciliacaoVM {

        // Atrbiutos Serviços
        private IReceitasDespesasVWBL _ReceitasDespesasVWBL;
        private IConciliacaoFinanceiraConsultaBL _ConciliacaoFinanceiraConsultaBL;
        
        // Propriedades Serviços
        private IReceitasDespesasVWBL OReceitasDespesasVWBL => _ReceitasDespesasVWBL = _ReceitasDespesasVWBL ?? new ReceitasDespesasVWBL();
        private IConciliacaoFinanceiraConsultaBL OConciliacaoFinanceiraConsultaBL => _ConciliacaoFinanceiraConsultaBL = _ConciliacaoFinanceiraConsultaBL ?? new ConciliacaoFinanceiraConsultaBL();
        
        // Filtros
        public DateTime? dtInicio { get; set; }
        public DateTime? dtFim { get; set; }
        public string pesquisarPor { get; set; }
        public DateTime? dtConciliacao { get; set; }
        public int? idContaBancaria { get; set; }
        public string valorBusca { get; set; }
        public string valorBuscaLote { get; set; }
        public string flagTipoTitulo { get; set; }
                
        public List<ReceitaDespesaVW> listaLancamentos { get; set; }
        public List<ConciliacaoFinanceira> listaConciliacao { get; set; }

        //
        public ConciliacaoVM() {
            this.listaLancamentos = new List<ReceitaDespesaVW>();
            this.listaConciliacao = new List<ConciliacaoFinanceira>();
        }

        //
        public void carregarLancamentos() {         
                        
            var query = this.OReceitasDespesasVWBL.listar().Where(x => x.dtPagamento != null && x.idPagamento > 0 && x.idConciliacao == null);
            
            if (pesquisarPor == "P") {

                query = query.Where(x => DbFunctions.TruncateTime(x.dtPagamento) >= this.dtInicio && DbFunctions.TruncateTime(x.dtPagamento) < this.dtFim);

            } else if (pesquisarPor == "V") {

                query = query.Where(x => DbFunctions.TruncateTime(x.dtVencimento) >= this.dtInicio && DbFunctions.TruncateTime(x.dtVencimento) < this.dtFim);

            } else {

                query = query.Where(x => DbFunctions.TruncateTime(x.dtPrevisaoEfetivacao) >= this.dtInicio && DbFunctions.TruncateTime(x.dtPrevisaoEfetivacao) < this.dtFim);
            }            
            
            if (this.idContaBancaria > 0) {
                query = query.Where(x => x.idContaBancaria == this.idContaBancaria);
            }

            if (!this.flagTipoTitulo.isEmpty()) {
                query = query.Where(x => x.flagTipoTitulo.Equals(this.flagTipoTitulo));
            }

            if (!this.valorBusca.isEmpty()) {

                query = query.Where(x => x.descricaoTitulo.Contains(this.valorBusca) || x.nomePessoa.Contains(this.valorBusca));
            }
            
            if (!valorBuscaLote.isEmpty()) {

                string[] separadores = { "\r\n" };
                string[] valoresBusca = valorBuscaLote.Split(separadores, StringSplitOptions.None).Where(x => !x.isEmpty()).ToArray();
                
                var valoresNumericos = valoresBusca.Select(x => (int?) x.toInt()).Where(x => x > 0).ToList();

                var valoresSoNumeros = valoresBusca.Select(UtilString.onlyNumber).Where(x => !x.isEmpty()).ToList();

                query = query.Where(x => valoresNumericos.Contains(x.idPagamento) ||
                                         valoresNumericos.Contains(x.idTitulo) ||
                                         valoresSoNumeros.Contains(x.tokenTransacao));
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
                x.valorRealizado, 
                x.valorTarifasBancarias,
                x.valorTarifasTransacao,
                x.dtPagamento, 
                x.dtEfetivacao, 
                x.dtPrevisaoEfetivacao    
             }).ToListJsonObject<ReceitaDespesaVW>();

            foreach (var Item in listaLancamentos) {

                Item.descricaoTitulo = $"{Item.nomePessoa} - {Item.descricaoTitulo}";
                Item.valorRealizado = Item.valorRealizado ?? 0;

                Item.dtMovimento = Item.dtPrevisaoEfetivacao;
                
                if (pesquisarPor == "P") {

                    Item.dtMovimento = Item.dtPagamento;

                } else if (pesquisarPor == "V") {

                    Item.dtMovimento = Item.dtVencimento;

                }           
            }

            listaLancamentos = listaLancamentos.OrderBy(x => x.dtMovimento).ToList();
        }

        public void carregarConciliacoes()
        {
            if (!dtConciliacao.HasValue) this.dtConciliacao = DateTime.Today;
            
            var query = this.OConciliacaoFinanceiraConsultaBL.listar(this.dtConciliacao, 0);

            this.listaConciliacao = query.ToList();
            this.listaConciliacao.ForEach(item => {
                item.listaConciliacaoFinanceiraDetalhe.ForEach(subItem => {
                    
                    if (subItem.idTituloDespesaPagamento > 0)
                    {
                        item.valorTotal = item.valorTotal - subItem.valorConciliado;
                    }
                    
                    if (subItem.idTituloReceitaPagamento > 0){
                        item.valorTotal = item.valorTotal + subItem.valorConciliado;
                    }
                });
            });
        }

        public void carregarPeriodo()
        {
              
            if (!this.dtInicio.HasValue && !this.dtFim.HasValue) {

                this.dtInicio = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);

                this.dtFim = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.DaysInMonth(DateTime.Today.Year, DateTime.Today.Month));
            }                         

            if (this.dtInicio.HasValue && !this.dtFim.HasValue) {
                this.dtFim = this.dtInicio.Value.AddDays(31);
            }

            if(this.dtFim.HasValue && !this.dtInicio.HasValue) {
                this.dtInicio = this.dtFim.Value.AddDays(-31);
            }
            
            if (this.dtInicio.HasValue && this.dtFim.HasValue) {
                this.dtFim = this.dtFim.Value.AddDays(1);
            }
            
            if (!this.dtConciliacao.HasValue) {
                this.dtConciliacao = DateTime.Today;
            }            
        }
    }
}