using System;
using System.Data.Entity;
using System.Linq;
using DAL.Eventos;
using EntityFramework.Extensions;
using BLL.Services;
using DAL.Permissao.Security.Extensions;
using DAL.Publicacoes;

namespace BLL.Publicacoes {

	public class ConteudoExclusaoBL : DefaultBL, IConteudoExclusaoBL {
		
		//
		public ConteudoExclusaoBL() {
		}                       

        //Remover um registro (exclusao lógica - nao remove-se fisicamente)
        public bool excluir(int id) {            

            db.Conteudo
                .Where(x => x.id == id)
                .Update(x => new Conteudo { dtExclusao = DateTime.Now, dtAlteracao = DateTime.Now,idUsuarioAlteracao = User.id(), idUsuarioExclusao = User.id() });
            return true;
        }
		
		public bool excluir(int[] ids) {
			db.Conteudo.Where(x => ids.Contains(x.id) && x.dtExclusao == null)
				.Update(x => new Conteudo { dtExclusao = DateTime.Now, dtAlteracao = DateTime.Now, idUsuarioAlteracao = User.id(), idUsuarioExclusao = User.id() });
				//.Delete();

			var listaCheck = db.Conteudo.Where(x => ids.Contains(x.id) && x.dtExclusao == null).ToList();
			return (listaCheck.Count == 0);

		}
	}
}