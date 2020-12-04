using System;
using System.Linq;
using DAL.Associados;

namespace BLL.Associados {

	public interface IConfiguracaoMembroConsultaBL {
		
		//*Rotinas de consulta*//
		IQueryable<ConfiguracaoMembro> query(int? idOrganizacaoParam = null);
		
		ConfiguracaoMembro carregar(int id);

		ConfiguracaoMembro carregarPorMembro(int idMembro);
		
		IQueryable<ConfiguracaoMembro> listar(bool? ativo);
		
	}
}