using System.Linq;
using DAL.Configuracoes;

namespace BLL.Configuracoes {

    public interface IConfiguracaoTipoCampoBL {

        /// <summary>
        /// Carregar registro pelo ID
        /// </summary>
        ConfiguracaoTipoCampo carregar(int id);

        /// <summary>
        /// Montagem de query LINQ
        /// </summary>
        IQueryable<ConfiguracaoTipoCampo> listar(string descricao, bool? ativo = true);

        /// <summary>
        /// Incluir ou atualizar um registro em base de dados
        /// </summary>
        bool salvar(ConfiguracaoTipoCampo OConfiguracaoTipoCampo);
    }
}