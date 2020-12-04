using System.Web.Mvc;
using DAL.ConfiguracoesAssociados;

namespace BLL.ConfiguracoesAssociados.Extensions{

    public static class ConfiguracaoAreaAssociadoExtensions{

        /// <summary>
        /// Carregar as configuracoes para area do associado
        /// </summary>
        public static string descricaoTipoCadastro(this HtmlHelper helper, short? idTipoCadastro) {

            if (TipoCampoCadastroConst.DP == idTipoCadastro) {
                return "Dependente";
            }
            if (TipoCampoCadastroConst.NA_DP == idTipoCadastro) {
                return "Não Associado Dependente";
            }
            if (TipoCampoCadastroConst.NA_PF == idTipoCadastro) {
                return "Não Associado Pessoa Física";
            }
            if (TipoCampoCadastroConst.NA_PJ == idTipoCadastro) {
                return "Não Associado Pessoa Jurídica";
            }
            if (TipoCampoCadastroConst.PF == idTipoCadastro) {
                return "Associado Pessoa Física";
            }
            if (TipoCampoCadastroConst.PJ == idTipoCadastro) {
                return "Associado Pessoa Jurídica";
            }

            return "";

        }


    }
}