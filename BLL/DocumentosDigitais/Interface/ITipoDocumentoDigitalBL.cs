using DAL.DocumentosDigitais;
using System.Linq;

namespace BLL.DocumentosDigitais {

    public interface ITipoDocumentoDigitalBL {
		TipoDocumentoDigital carregar(int id);
		IQueryable<TipoDocumentoDigital> listar(string valorBusca, bool? ativo);
        TipoDocumentoDigital salvar(TipoDocumentoDigital OTipoDocumentoDigital);
		bool excluir(int id);
	}
}