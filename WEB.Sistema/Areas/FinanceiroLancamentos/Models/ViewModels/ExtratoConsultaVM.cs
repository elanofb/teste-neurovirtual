using System;
using System.Collections.Generic;
using System.Linq;
using BLL.FinanceiroLancamentos;
using DAL.Financeiro;
using MoreLinq;
using WEB.Areas.FinanceiroLancamentos.ViewModels;

namespace WEB.Areas.LancamentoRecebimentos.ViewModels {

    public class ExtratoConsultaVM {
                
        //Atributos Serviços
        private ITituloDespesaPagamentoResumoVWBL _TituloDespesaPagamentoResumoVWBL;
        private ITituloReceitaPagamentoResumoVWBL _TituloReceitaPagamentoResumoVWBL;

        //Propriedades Serviços
        private ITituloDespesaPagamentoResumoVWBL OTituloDespesaPagamentoResumoVWBL => _TituloDespesaPagamentoResumoVWBL = _TituloDespesaPagamentoResumoVWBL ?? new TituloDespesaPagamentoResumoVWBL();
        private ITituloReceitaPagamentoResumoVWBL OTituloReceitaPagamentoResumoVWBL => _TituloReceitaPagamentoResumoVWBL = _TituloReceitaPagamentoResumoVWBL ?? new TituloReceitaPagamentoResumoVWBL();
        
        // Propriedades
        public string valorBusca { get; set; }
        
        public int? idContaBancaria { get; set; }
        
        public string flagPago { get; set; }
            
        public string flagTipoSaida { get; set; }
        
        public string valorBuscaLote { get; set; }
        
        public string pesquisarPor { get; set; }
        
        public DateTime? dtInicio { get; set; }
        
        public DateTime? dtFim { get; set; }

        public List<int> idsTipoReceita { get; set; }
        
        public int? idCentroCusto { get; set; }

        public int? idMacroConta { get; set; }

        public byte? idGateway { get; set; }
        
        public byte? idMeioPagamento { get; set; }

        public string flagTipoBaixa { get; set; }

        public List<TituloPagamentoResumoDTO> listaTituloDespesaPagamento { get; set; }
        
        public List<TituloPagamentoResumoDTO> listaTituloReceitaPagamento { get; set; }
        
        public List<TituloPagamentoResumoDTO> listaTituloPagamento { get; set; }

        public List<TituloPagamentoResumoPessoaDTO> listaPessoas { get; set; }

        // Totalizadores
        public decimal totalDespesasPagas { get; set; }
        
        public decimal totalDespesasAPagar { get; set; }
        
        public decimal totalReceitasRecebidas { get; set; }

        public decimal? totalReceitasLiquidaRecebidas { get; set; }
        
        public decimal totalReceitasAReceber { get; set; }

        //
        public ExtratoConsultaVM() {
            
            this.listaTituloDespesaPagamento = new List<TituloPagamentoResumoDTO>();
            
            this.listaTituloReceitaPagamento = new List<TituloPagamentoResumoDTO>();

            this.listaPessoas = new List<TituloPagamentoResumoPessoaDTO>();

            this.idsTipoReceita = new List<int>();

        }
        
        //
        public void carregarInformacoes() {
            
            this.dtInicio = this.dtInicio ?? new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            this.dtFim = this.dtFim ?? new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month));
            
            this.carregarDadosReceitas();
            
            this.carregarDadosDespesas();
            
            this.listaTituloPagamento = new List<TituloPagamentoResumoDTO>();
            
            this.listaTituloPagamento.AddRange(this.listaTituloReceitaPagamento);
            
            this.listaTituloPagamento.AddRange(this.listaTituloDespesaPagamento);

            this.listaTituloPagamento = this.listaTituloPagamento.OrderBy(x => x.dtVencimento).ToList();
            
            this.carregarListaPessoas();
        }

        //
        private void carregarDadosReceitas() {
            
            var queryReceita = this.OTituloReceitaPagamentoResumoVWBL.listarPagamentoReceitas(this.valorBusca, this.idCentroCusto.toInt(), this.idMacroConta.toInt(), this.idContaBancaria.toInt(), 0, this.flagPago, this.pesquisarPor, this.dtInicio, this.dtFim);
            
            if (this.idsTipoReceita.Any()) {

                var ids = this.idsTipoReceita.Select(x => x.toByte()).ToList();

                queryReceita = queryReceita.Where(x => ids.Contains(x.idTipoReceita.Value));
            }

            if (this.idGateway > 0) {

                queryReceita = queryReceita.Where(x => x.idGatewayPagamento == this.idGateway);

            }

            if (this.idMeioPagamento > 0) {

                queryReceita = queryReceita.Where(x => x.idMeioPagamento == this.idMeioPagamento);
            }

            if (this.flagTipoBaixa == "M") {

                queryReceita = queryReceita.Where(x => x.flagBaixaAutomatica != true);
            }

            if (this.flagTipoBaixa == "A") {

                queryReceita = queryReceita.Where(x => x.flagBaixaAutomatica == true);
            }
            
            if (!this.valorBuscaLote.isEmpty()) {

                string[] separadores = { "\r\n" };
                string[] valoresBusca = this.valorBuscaLote.Split(separadores, StringSplitOptions.None).Where(x => !x.isEmpty()).ToArray();
                
                var valoresNumericos = valoresBusca.Select(x => (int?) x.toInt()).Where(x => x > 0).ToList();

                var valoresSoNumeros = valoresBusca.Select(UtilString.onlyNumber).Where(x => !x.isEmpty()).ToList();

                queryReceita = queryReceita.Where(x => valoresNumericos.Contains(x.idTituloPagamento) ||
                                         valoresNumericos.Contains(x.idTituloReceita) ||
                                         valoresSoNumeros.Contains(x.tokenTransacao) ||
                                         valoresSoNumeros.Contains(x.nroDocumentoPessoa));
            }
                        
            var listaReceitasRecebidas = queryReceita.Where(x => x.dtPagamento != null).ToList();
            
            this.totalReceitasRecebidas= listaReceitasRecebidas.Any() ? listaReceitasRecebidas.Sum(x => x.valorRecebido.toDecimal()) : 0;

            this.totalReceitasLiquidaRecebidas = listaReceitasRecebidas.Count > 0 ? listaReceitasRecebidas.Sum(x => x.valorLiquido()) : 0;

            var listaReceitasEmAberto = queryReceita.Where(x => x.dtPagamento == null).ToList();
            
            this.totalReceitasAReceber = listaReceitasEmAberto.Any() ? listaReceitasEmAberto.Sum(x => x.valorOriginal.toDecimal()) : 0;
            
            this.listaTituloReceitaPagamento = queryReceita.ToList().Select(x => new TituloPagamentoResumoDTO {
                                                   id = x.idTituloPagamento, idTitulo = x.idTituloReceita, flagTipoTitulo = "R",
                                                   descricao = x.descricao, descParcela = x.descricaoParcela, descricaoCentroCusto = x.descricaoCentroCusto,
                                                   idUnidade = x.idUnidade, siglaUnidade = x.siglaUnidade, dtVencimento = x.dtVencimentoRecebimento,
                                                   dtPagamento = x.dtPagamento, valorOriginal = x.valorComJurosEDescontos(), valorRealizado = x.valorRecebido,
                                                   idStatusPagamento = x.idStatusPagamento, descricaoStatusPagamento = x.descricaoStatusPagamento,
                                                   idPessoa = x.idPessoa, nomePessoa = x.nomePessoa, valorLiquido = x.valorLiquido(), 
                                                   dtPrevisaoCredito = x.dtPrevisaoCredito, dtCredito = x.dtCredito, idArquivoRemessa = x.idArquivoRemessa
                                               }).OrderBy(x => x.id).ToList();
            
        }
        
        //
        private void carregarDadosDespesas() {
            
            var queryDespesa = this.OTituloDespesaPagamentoResumoVWBL.listarPagamentoDespesas(this.valorBusca, idCentroCusto.toInt(), idMacroConta.toInt(), this.idContaBancaria.toInt(), this.flagPago, this.pesquisarPor, this.dtInicio, this.dtFim);
            
            if (!this.valorBuscaLote.isEmpty()) {

                string[] separadores = { "\r\n" };
                string[] valoresBusca = this.valorBuscaLote.Split(separadores, StringSplitOptions.None).Where(x => !x.isEmpty()).ToArray();
                
                var valoresNumericos = valoresBusca.Select(x => (int?) x.toInt()).Where(x => x > 0).ToList();

                var valoresSoNumeros = valoresBusca.Select(UtilString.onlyNumber).Where(x => !x.isEmpty()).ToList();

                queryDespesa = queryDespesa.Where(x => valoresNumericos.Contains(x.idTituloPagamento) ||
                                                       valoresNumericos.Contains(x.idTituloDespesa) ||
                                                       valoresSoNumeros.Contains(x.nroDocumentoPessoa));
            }
            
            var listaResumoDespesa = queryDespesa.Select(x => new { x.dtPagamento, x.dtVencimentoDespesa, x.valorOriginal, x.valorTotal, x.idTituloPagamento, x.valorPago }).ToList();
            
            var listaDespesasPagas = listaResumoDespesa.Where(x => x.dtPagamento != null).ToList();
            this.totalDespesasPagas = listaDespesasPagas.Any() ? listaDespesasPagas.Sum(x => x.valorOriginal.toDecimal()) : 0;
            
            var listaDespesasEmAberto = listaResumoDespesa.Where(x => x.dtPagamento == null).ToList();
            this.totalDespesasAPagar = listaDespesasEmAberto.Any() ? listaDespesasEmAberto.Sum(x => x.valorOriginal.toDecimal()) : 0;
           
            this.listaTituloDespesaPagamento = queryDespesa.Select(x => new TituloPagamentoResumoDTO {
                                                   id = x.idTituloPagamento, idTitulo = x.idTituloDespesa, flagTipoTitulo = "D",
                                                   descricao = x.descricao, descParcela = x.descParcela, descricaoCentroCusto = x.descricaoCentroCusto,
                                                   idUnidade = x.idUnidade, siglaUnidade = x.siglaUnidade, dtVencimento = x.dtVencimentoDespesa,
                                                   dtPagamento = x.dtPagamento, valorOriginal = x.valorOriginal, valorRealizado = x.valorPago,
                                                   idStatusPagamento = x.idStatusPagamento, descricaoStatusPagamento = x.descricaoStatusPagamento,
                                                   idPessoa = x.idPessoa, nomePessoa = x.nomePessoa, valorLiquido = x.valorPago, 
                                                   dtPrevisaoCredito = x.dtPagamento, dtCredito = x.dtPagamento, idArquivoRemessa = x.idArquivoRemessa            
                                               }).OrderBy(x => x.id).ToList();  
        }

        public void carregarListaPessoas() {
            
            this.listaPessoas = this.listaTituloPagamento
                                    .Select(x => new TituloPagamentoResumoPessoaDTO { id = x.idPessoa.toInt(), nome = x.nomePessoa })
                                    .DistinctBy(x => x.id).Where(x => x.id > 0).ToList();
            
        }

    }

}