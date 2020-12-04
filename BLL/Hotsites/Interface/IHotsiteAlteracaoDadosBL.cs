using System;

namespace BLL.Hotsites.Services {

    public interface IHotsiteAlteracaoDadosBL {

        UtilRetorno alterarCampo(int id, string nomeCampo, string novoValor, string nomeCampoDisplay = "", string oldValueSelect = "", string newValueSelect = "", string justificativa = "");

    }
}
