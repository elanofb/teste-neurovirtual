using System.Linq;
using DAL.Financeiro;
using System;

namespace BLL.Financeiro {

	public interface IPeriodoRepeticaoBL {
        PeriodoRepeticao carregar(int id);
        IQueryable<PeriodoRepeticao> listar(string valorBusca,string ativo);
        bool existe(string descricao,int id);
        bool salvar(PeriodoRepeticao OTipoProduto);
        bool excluir(int id);
	    DateTime getNewDatePeriodo(DateTime data, int idPeriodoRepeticao);
	}
}
