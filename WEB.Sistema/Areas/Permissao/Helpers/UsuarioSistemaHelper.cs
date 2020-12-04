using System.Linq;
using System.Web.Mvc;
using BLL.Permissao;

namespace WEB.Areas.Permissao.Helpers {

	public class UsuarioSistemaHelper {

        //Atributos
	    private static UsuarioSistemaHelper _instance;
        private UsuarioSistemaBL _UsuarioSistemaBL;

        //Propriedades
        public static UsuarioSistemaHelper getInstance => _instance = _instance ?? new UsuarioSistemaHelper();
	    private UsuarioSistemaBL OUsuarioSistemaBL => _UsuarioSistemaBL = _UsuarioSistemaBL ?? new UsuarioSistemaBL();

        //Construtor
	    private UsuarioSistemaHelper() {
	        
	    }

		//
		public SelectList selectList(int? selected, int idPerfilAcesso) {

			var query = OUsuarioSistemaBL.listar(idPerfilAcesso, "", "S").Where(x => x.id != 1);

			var listaUsuarios = query.OrderBy(x => x.nome).ToList();

			return new SelectList(listaUsuarios, "id", "nome", selected);
		}
		
		//
		public SelectList selectListPorUnidade(int? selected, int idPerfilAcesso, int idUnidade) {

			var query = OUsuarioSistemaBL.listar(idPerfilAcesso, "", "S").Where(x => x.id != 1);

			if (idUnidade > 0)
			{
				query = query.Where(x => x.listaUsuarioUnidade.Any(i => i.idUnidade == idUnidade));	
			}			
				
			var listaUsuarios = query.OrderBy(x => x.nome).ToList();

			return new SelectList(listaUsuarios, "id", "nome", selected);
		}
		
	}
}