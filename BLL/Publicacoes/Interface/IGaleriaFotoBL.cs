using System.Linq;
using DAL.Publicacoes;
using System.Web;
using System.Collections.Generic;
using System.Json;

namespace BLL.Publicacoes {

	public interface IGaleriaFotoBL {

	    IQueryable<GaleriaFoto> query(int? idOrganizacaoParam = null);

        GaleriaFoto carregar(int id);

	    IQueryable<GaleriaFoto> listar(string valorBusca, string ativo, int idTipoGaleria = 0, bool flagImagemAtiva = false);

        bool salvar(GaleriaFoto OGaleriaFoto, List<HttpPostedFileBase> listaArquivos);

        JsonMessageStatus alterarStatus(int id);

        bool excluir(int[] ids);
	}
}
