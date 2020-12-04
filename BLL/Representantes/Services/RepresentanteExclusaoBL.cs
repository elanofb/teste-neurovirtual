using System;
using System.Linq;
using EntityFramework.Extensions;
using BLL.Services;
using DAL.Permissao.Security.Extensions;
using DAL.Representantes;

namespace BLL.Representantes {

	public class RepresentanteExclusaoBL : DefaultBL, IRepresentanteExclusaoBL {
		
		//
		public RepresentanteExclusaoBL() {
		}                       

        //Remover um registro (exclusao lógica - nao remove-se fisicamente)
        public bool excluir(int id) {            

            db.Representante
                .Where(x => x.id == id)
                .Update(x => new Representante { dtExclusao = DateTime.Now, idUsuarioExclusao = User.id() });
	        
            return true;
	        
        }
		
		public bool excluir(int[] ids) {
			
			db.Representante.Where(x => ids.Contains(x.id) && x.dtExclusao == null)
				.Update(x => new Representante { dtExclusao = DateTime.Now, idUsuarioExclusao = User.id() });

			var listaCheck = db.Representante.Where(x => ids.Contains(x.id) && x.dtExclusao == null).ToList();
			
			return (listaCheck.Count == 0);

		}
	}
}