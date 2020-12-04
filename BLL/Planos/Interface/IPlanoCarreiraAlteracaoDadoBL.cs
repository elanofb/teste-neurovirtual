using System;

namespace BLL.Planos.Services{
    
    public interface IPlanoCarreiraAlteracaoDadoBL{
        
        /// <summary>
        /// Alteração de campos
        /// </summary>
        UtilRetorno alterarCampo(int id, string nomeCampo, string novoValor, string nomeCampoDisplay = "", string oldValueSelect = "", string newValueSelect = "", string justificativa = "");
        
    }
}