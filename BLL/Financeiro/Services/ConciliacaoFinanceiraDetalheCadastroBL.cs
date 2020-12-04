using System;
using DAL.Financeiro;

namespace BLL.Financeiro {

    public class ConciliacaoFinanceiraDetalheCadastroBL : ConciliacaoFinanceiraDetalheConsultaBL, IConciliacaoFinanceiraDetalheCadastroBL {

	    //Verificar se deve-se atualizar um registro existente ou criar um novo
	    public bool salvar(ConciliacaoFinanceiraDetalhe OConciliacaoFinanceiraDetalhe) {
		    
		    if (OConciliacaoFinanceiraDetalhe.id == 0) {
			    return this.inserir(OConciliacaoFinanceiraDetalhe);
		    }

		    return this.atualizar(OConciliacaoFinanceiraDetalhe);
	    }

	    //Persistir o objecto e salvar na base de dados
	    private bool inserir(ConciliacaoFinanceiraDetalhe OConciliacaoFinanceiraDetalhe) {
            
		    OConciliacaoFinanceiraDetalhe.setDefaultInsertValues<ConciliacaoFinanceiraDetalhe>();
		    db.ConciliacaoFinanceiraDetalhe.Add(OConciliacaoFinanceiraDetalhe);
		    db.SaveChanges();
		    return (OConciliacaoFinanceiraDetalhe.id > 0);
	    }

	    //Persistir o objecto e atualizar informações
	    private bool atualizar(ConciliacaoFinanceiraDetalhe OConciliacaoFinanceiraDetalhe)
	    {

		    ConciliacaoFinanceiraDetalhe dbConciliacaoFinanceiraDetalhe = this.carregar(OConciliacaoFinanceiraDetalhe.id);

		    if (dbConciliacaoFinanceiraDetalhe == null){
			    return false;
		    }

		    var tipoEntry = db.Entry(dbConciliacaoFinanceiraDetalhe);

		    OConciliacaoFinanceiraDetalhe.setDefaultUpdateValues<ConciliacaoFinanceiraDetalhe>();
		    tipoEntry.CurrentValues.SetValues(OConciliacaoFinanceiraDetalhe);

		    db.SaveChanges();
		    return (OConciliacaoFinanceiraDetalhe.id > 0);
	    }
    }
}