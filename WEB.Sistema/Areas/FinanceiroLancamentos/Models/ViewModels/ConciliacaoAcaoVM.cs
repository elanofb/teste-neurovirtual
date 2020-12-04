using System;
using System.Collections.Generic;
using System.Linq;
using BLL.Financeiro;
using DAL.Financeiro;

namespace WEB.Areas.FinanceiroLancamentos.ViewModels{
    
	public class ConciliacaoAcaoVM{

		// Atrbiutos Serviços
		private IReceitasDespesasVWBL _ReceitasDespesasVWBL;
        
		// Propriedades Serviços
		private IReceitasDespesasVWBL OReceitasDespesasVWBL => _ReceitasDespesasVWBL = _ReceitasDespesasVWBL ?? new ReceitasDespesasVWBL();

	    //Construtor
		public ConciliacaoAcaoVM(){}
		
		public List<ConciliacaoFinanceira> gerarListaConciliacao(ConciliacaoAcaoForm viewModel)
	    {
		    var lista = new List<ConciliacaoFinanceira>();
		    var listaPagamentos = OReceitasDespesasVWBL.listar().Where(x => viewModel.idsLancamentos.Contains(x.idPagamento)).ToList();
		    
		    for (int x = 0; x < viewModel.idsLancamentos.Count; x++)
		    {
			    var idPagamento = viewModel.idsLancamentos[x];
			    var flagTipoTitulo = viewModel.tiposLancamentos[x];
			    var OReceitaDespesa = listaPagamentos.FirstOrDefault(i => i.idPagamento == idPagamento && i.flagTipoTitulo == flagTipoTitulo);

			    if (OReceitaDespesa == null){
				    continue;
			    }
			    
			    var OConciliacaoFinanceira = new ConciliacaoFinanceira();
			    OConciliacaoFinanceira.descricao = viewModel.descricao;
			    OConciliacaoFinanceira.dtConciliacao = viewModel.dtConciliacao;
			    OConciliacaoFinanceira.listaConciliacaoFinanceiraDetalhe = new List<ConciliacaoFinanceiraDetalhe>();

			    var OConciliacaoFinanceiraDetalhe = new ConciliacaoFinanceiraDetalhe();
			    OConciliacaoFinanceiraDetalhe.idTituloReceitaPagamento = null;
			    OConciliacaoFinanceiraDetalhe.idTituloDespesaPagamento = OReceitaDespesa.idPagamento;
			    OConciliacaoFinanceiraDetalhe.valorConciliado = (OReceitaDespesa.valorRealizado - (OReceitaDespesa.valorTarifasTransacao + OReceitaDespesa.valorTarifasBancarias)).toDecimal();
			    
			    if (OReceitaDespesa.flagTipoTitulo == "R"){
				    
				    OConciliacaoFinanceiraDetalhe.idTituloReceitaPagamento = OReceitaDespesa.idPagamento;
				    OConciliacaoFinanceiraDetalhe.idTituloDespesaPagamento = null;
			    }
			    
			    OConciliacaoFinanceira.listaConciliacaoFinanceiraDetalhe.Add(OConciliacaoFinanceiraDetalhe);
			    lista.Add(OConciliacaoFinanceira);
		    }

		    return lista;
	    }
	    
	    public List<ConciliacaoFinanceira> gerarListaConciliacaoAgrupada(ConciliacaoAcaoForm viewModel)
	    {
		    var lista = new List<ConciliacaoFinanceira>();
		    
		    var OConciliacaoFinanceira = new ConciliacaoFinanceira();
		    OConciliacaoFinanceira.descricao = viewModel.descricao;
		    OConciliacaoFinanceira.dtConciliacao = viewModel.dtConciliacao;
		    OConciliacaoFinanceira.listaConciliacaoFinanceiraDetalhe = new List<ConciliacaoFinanceiraDetalhe>();
		    
		    var listaPagamentos = OReceitasDespesasVWBL.listar().Where(x => viewModel.idsLancamentos.Contains(x.idPagamento)).ToList();

		    for (int x = 0; x < viewModel.idsLancamentos.Count; x++)
		    {
			    var idPagamento = viewModel.idsLancamentos[x];
			    var flagTipoTitulo = viewModel.tiposLancamentos[x];
			    var OReceitaDespesa = listaPagamentos.FirstOrDefault(i => i.idPagamento == idPagamento && i.flagTipoTitulo == flagTipoTitulo);

			    if (OReceitaDespesa == null){
				    continue;
			    }
			    			    
			    var OConciliacaoFinanceiraDetalhe = new ConciliacaoFinanceiraDetalhe();
			    OConciliacaoFinanceiraDetalhe.idTituloReceitaPagamento = null;
			    OConciliacaoFinanceiraDetalhe.idTituloDespesaPagamento = OReceitaDespesa.idPagamento;
			    OConciliacaoFinanceiraDetalhe.valorConciliado = (OReceitaDespesa.valorRealizado - (OReceitaDespesa.valorTarifasTransacao + OReceitaDespesa.valorTarifasBancarias)).toDecimal();
			    
			    if (OReceitaDespesa.flagTipoTitulo == "R"){
				    
				    OConciliacaoFinanceiraDetalhe.idTituloReceitaPagamento = OReceitaDespesa.idPagamento;
				    OConciliacaoFinanceiraDetalhe.idTituloDespesaPagamento = null;
			    }
			    
			    OConciliacaoFinanceira.listaConciliacaoFinanceiraDetalhe.Add(OConciliacaoFinanceiraDetalhe);
		    }

		    lista.Add(OConciliacaoFinanceira);
		    
		    return lista;
	    }
    }

}