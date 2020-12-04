using System;

namespace BLL.LogsAlteracoes {

	public interface ILogTituloDespesaBL {

        UtilRetorno alterarCampo(int id, string nomeCampo, string novoValor, string justificativa, string nomeCampoDisplay = "", string oldValueSelect = "", string newValueSelect = "");

	}
}
