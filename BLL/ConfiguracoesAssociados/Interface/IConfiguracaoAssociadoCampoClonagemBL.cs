
namespace BLL.ConfiguracoesAssociados {

    public interface IConfiguracaoAssociadoCampoClonagemBL {
        /// <summary>
        /// Clonar todos os registros de configurações de campos do associado para a organização logada
        /// </summary>
        bool clonarDefaultSistema(int idOrganizacaoInf, int idTipoCampoCadastro);
    }
}