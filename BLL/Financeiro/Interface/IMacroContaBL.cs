using System.Linq;
using DAL.Financeiro;
using System.Json;
using System.Collections.Generic;

namespace BLL.Financeiro {

	public interface IMacroContaBL {

        MacroConta carregar(int id);

        IQueryable<MacroConta> listar(string valorBusca, bool? ativo, int idCentroCusto = 0);

        List<ReferenciaReceitaDTO> listarPorMacroConta(int idMacroConta);

        ReferenciaReceitaDTO getReferenciaReceita(int idMacroConta, int idReferenciaReceita);

        bool existe(string descricao,int id);

        bool salvar(MacroConta OTipoProduto);

        bool excluir(int id);

        JsonMessageStatus alterarStatus(int id);
	}
}
