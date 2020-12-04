using System.Json;
using DAL.Hotsites;

namespace BLL.Hotsites {

	public interface IHotsiteCadastroBL {

		bool salvar(Hotsite OHotsite);

		JsonMessageStatus alterarStatus(int id);

		bool salvarDadosIniciais(Hotsite OHotsite);

		bool salvarDadosFormatacao(Hotsite OHotsite);

		bool salvarDadosCustomizacao(Hotsite OHotsite);

		bool salvarBannerPrincipal(Hotsite OHotsite);
	}
}
