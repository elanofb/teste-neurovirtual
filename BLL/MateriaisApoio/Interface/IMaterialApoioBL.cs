using System;
using System.Linq;
using DAL.MateriaisApoio;
using System.Threading.Tasks;
using System.Web;

namespace BLL.MateriaisApoio {

	public interface IMaterialApoioBL {

		IQueryable<MaterialApoio> query(int? idOrganizacaoParam = null);
		
		MaterialApoio carregar(int id);
		
		IQueryable<MaterialApoio> listar(string valorBusca, string ativo);
		
		bool salvar(MaterialApoio OMaterialApoio, HttpPostedFileBase OArquivo);
		
		UtilRetorno excluir(int id);
		
	}
	
}
