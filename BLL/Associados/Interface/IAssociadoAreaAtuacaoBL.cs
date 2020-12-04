using System;
using System.Collections.Generic;
using System.Linq;
using DAL.Associados;

namespace BLL.Associados {

	public interface IAssociadoAreaAtuacaoBL{
		AssociadoAreaAtuacao carregar(int id);
		IQueryable<AssociadoAreaAtuacao> listar(int idAssociado, string ativo);
	    UtilRetorno salvarLote(List<int> idsAreaAtuacao, int idAssociado);
        bool salvar(AssociadoAreaAtuacao OAssociadoAreaAtuacao);
		bool existe(AssociadoAreaAtuacao OAssociadoAreaAtuacao, int idDesconsiderar);
		UtilRetorno excluir(int idAreaAtuacao, int idAssociado);
	    UtilRetorno excluirPorId(int id);
	}
}