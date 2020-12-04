using System.Linq;
using DAL.Telefones;

namespace BLL.Telefones
{
    public interface ITipoTelefoneBL
    {
        /// <summary>
        /// Carregar um registro pelo ID
        /// </summary>
        TipoTelefone carregar(int id);

        /// <summary>
        /// Listagem de registros de acordo com parametros informados
        /// </summary>
        IQueryable<TipoTelefone> listar(string descricao, bool? ativo = true);

        /// <summary>
        /// Persistir e salvar os dados
        /// </summary>
        bool salvar(TipoTelefone OTipoTelefone);
    }
}