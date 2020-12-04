using System.Linq;
using DAL.OrganizacaoConfiguracoes;
using DAL.Organizacoes;

namespace BLL.OrganizacaoConfiguracoes {

	public interface IOrganizacaoDadosAssociadoBL {

		OrganizacaoDadosAssociado carregar(int idOrganizacao = 0, bool flagCache = true);
        
		IQueryable<OrganizacaoDadosAssociado> listar(int idOrganizacao);

        bool salvar(OrganizacaoDadosAssociado OOrganizacaoDadosAssociado);

	}
}