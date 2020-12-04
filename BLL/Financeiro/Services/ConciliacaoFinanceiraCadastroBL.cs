using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Metadata.Edm;
using System.Linq;
using System.Linq.Dynamic;
using DAL.Financeiro;
using DAL.Permissao.Security.Extensions;
using EntityFramework.Extensions;

namespace BLL.Financeiro {

    public class ConciliacaoFinanceiraCadastroBL : ConciliacaoFinanceiraConsultaBL, IConciliacaoFinanceiraCadastroBL {

	    //Verificar se deve-se atualizar um registro existente ou criar um novo
	    public bool salvar(ConciliacaoFinanceira OConciliacaoFinanceira) {
		                
		    OConciliacaoFinanceira.setDefaultInsertValues<ConciliacaoFinanceira>();
		    OConciliacaoFinanceira.idOrganizacao = User.idOrganizacao();
		    OConciliacaoFinanceira.listaConciliacaoFinanceiraDetalhe.ForEach(x =>
		    {
			    x.setDefaultInsertValues();
			    x.idOrganizacao = User.idOrganizacao();
		    });
		    
		    db.ConciliacaoFinanceira.Add(OConciliacaoFinanceira);
		    
		    db.SaveChanges();

		    var retorno = false;

		    if (OConciliacaoFinanceira.id > 0)
		    {
			    OConciliacaoFinanceira.listaConciliacaoFinanceiraDetalhe.ForEach(item =>
			    {
				    var flagReceita = true;
				    var idLancamneto = UtilNumber.toInt32(item.idTituloReceitaPagamento);
				    
				    if (item.idTituloDespesaPagamento > 0){
					    flagReceita = false;
					    idLancamneto = UtilNumber.toInt32(item.idTituloDespesaPagamento);					    
				    }

				    this.atualizaLancamentos(flagReceita, idLancamneto, OConciliacaoFinanceira.id, item.ConciliacaoFinanceira.dtConciliacao);
			    });			    
			    
			    retorno = true;
		    }

		    return retorno;
	    }
	    
	    public UtilRetorno excluir(ConciliacaoFinanceira OConciliacaoFinanceira) {
		    
		    OConciliacaoFinanceira.listaConciliacaoFinanceiraDetalhe.ForEach(item =>
		    {
			    var flagReceita = true;
			    var idLancamneto = UtilNumber.toInt32(item.idTituloReceitaPagamento);
				    
			    if (item.idTituloDespesaPagamento > 0){
				    flagReceita = false;
				    idLancamneto = UtilNumber.toInt32(item.idTituloDespesaPagamento);					    
			    }

			    this.atualizaLancamentos(flagReceita, idLancamneto, null, null);
		    });			    
		    

		    var idUsuario = User.id();

		    db.ConciliacaoFinanceira.Where(x => x.id == OConciliacaoFinanceira.id).condicoesSeguranca()
			    .Update(x => new ConciliacaoFinanceira { dtExclusao = DateTime.Now, idUsuarioExclusao = idUsuario });

		    return UtilRetorno.newInstance(false);
	    }

	    private void atualizaLancamentos(bool flagReceita, int idLancamento, int? idConciliacao, DateTime? dtConciliacao)
	    {
		    var idUsuario = User.id();
				    
		    if (flagReceita){
			    
			    db.TituloReceitaPagamento
				    .Where(x => x.id == idLancamento).condicoesSeguranca()
				    .Update(x => new TituloReceitaPagamento
				    {
					    idConciliacao = idConciliacao, 
					    dtConciliacao = dtConciliacao, 
					    dtCredito = dtConciliacao,
					    idUsuarioAlteracao = idUsuario,
					    dtAlteracao = DateTime.Today
				    });
			    
		    }else{

			    db.TituloDespesaPagamento
					    
				    .Where(x => x.id == idLancamento).condicoesSeguranca()
				    .Update(x => new TituloDespesaPagamento
				    {
					    idConciliacao = idConciliacao, 
					    dtConciliacao = dtConciliacao,
					    idUsuarioAlteracao = idUsuario,
					    dtAlteracao = DateTime.Today,
				    });    			        
		    }
	    }	    

    }
}