using System.Collections.Generic;

namespace BLL.Arquivos {

	public interface IExportarArquivoAssociadoVWBL {

        string exportar(List<string> listaUrlArquivosAssociados);
    }
}
