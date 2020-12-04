using System.Linq;
using DAL.Arquivos;
using System.Collections.Generic;

namespace BLL.Arquivos {

	public interface IArquivoAssociadoVWBL {

	    ArquivoAssociadoVW carregar(int id);

		IQueryable<ArquivoAssociadoVW> listar(List<int> idsTipoAssociado, string flagSituacaoContribuicao, string buscaAssociado, int idEntidadeArquivo, string formatoArquivo, string valorBusca, string ativo);

        List<string> carregarArquivosExportacao(List<int> idsArquivos);

    }
}
