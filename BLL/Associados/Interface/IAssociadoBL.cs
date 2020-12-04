using System.Linq;
using DAL.Associados;

namespace BLL.Associados {

	public interface IAssociadoBL {

	    IQueryable<Associado> query(int? idOrganizacaoParam = null);

		Associado carregar(int id);
        
        Associado carregarAssociadoPessoa(int idPessoa);

        IQueryable<Associado> listar(int idTipoAssociado, string flagSituacao, string valorBusca, string ativo);

        IQueryable<AssociadoAutoComplete> autocompletar(string valorBusca, int idPessoa);

        bool existe(int idTipoDocumento, string cpf, string email, string login, byte idTipoCadastro, int idDesconsiderado, int? idOrganizacaoParam = null);

	    bool existeLogin(string login, int id, int? idOrganizacaoParam = null);

		bool existeRota(string rota, int id, int? idOrganizacaoParam = null);

		bool existeCodigo(string codigo, int id, int? idOrganizacaoParam = null);
		
		bool cryptSenha(int id, string senhaClean);

	}
}
