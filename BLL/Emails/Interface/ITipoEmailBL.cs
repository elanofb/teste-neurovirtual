using System.Linq;
using DAL.Emails;

namespace BLL.Emails
{
    public interface ITipoEmailBL
    {
        /// <summary>
        /// Carregar um registro pelo ID
        /// </summary>
        TipoEmail carregar(int id);

        /// <summary>
        /// Listagem de registros de acordo com parametros informados
        /// </summary>
        IQueryable<TipoEmail> listar(string descricao, bool? ativo = true);

        /// <summary>
        /// Persistir e salvar os dados
        /// </summary>
        bool salvar(TipoEmail OTipoEmail);
    }
}