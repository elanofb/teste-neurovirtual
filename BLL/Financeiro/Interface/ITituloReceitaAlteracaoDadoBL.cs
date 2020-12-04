using System;

namespace BLL.Financeiro.Interface {
    public interface ITituloReceitaAlteracaoDadoBL {

        UtilRetorno alterarCampo(int id, string nomeCampo, string novoValor, string nomeCampoDisplay = "", string oldValueSelect = "", string newValueSelect = "", string justificativa = "");
    }
}