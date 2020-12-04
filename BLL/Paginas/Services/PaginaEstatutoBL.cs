using System;
using System.Data.Entity;
using System.Linq;
using BLL.Services;
using DAL.Paginas;
using DAL.Permissao.Security.Extensions;
using EntityFramework.Extensions;

namespace BLL.Paginas {

	public class PaginaEstatutoBL: DefaultBL, IPaginaEstatutoBL {

        //Constantes
        private static IPaginaEstatutoBL _instance;

        //Propriedades
        public static IPaginaEstatutoBL getInstance => _instance = _instance ?? new PaginaEstatutoBL();

        //
        public PaginaEstatutoBL(){

		}
        
        //
		public PaginaEstatuto carregar(int idOrganizacaoParam = 0) {

		    if (User.idOrganizacao() > 0){
		        idOrganizacaoParam = User.idOrganizacao();
		    }
            
			var query = db.PaginaEstatuto
                          .Include(x => x.Organizacao).Include(x => x.UsuarioCadastro)
						  .Where(x => x.dtExclusao == null);

		    query = idOrganizacaoParam > 0 ? query.Where(x => x.idOrganizacao == idOrganizacaoParam) : query.Where(x => x.idOrganizacao == null);

            var OPaginaEstatuto = query.OrderByDescending(x => x.id).FirstOrDefault();
            
		    return OPaginaEstatuto;

		}
        
		//
		public IQueryable<PaginaEstatuto> listar(int idOrganizacao) {

			var query = db.PaginaEstatuto
                          .Include(x => x.Organizacao).Include(x => x.Organizacao.Pessoa).Include(x => x.UsuarioCadastro)
						  .Where(x => x.dtExclusao == null).AsNoTracking();

    	    if (idOrganizacao > 0) {
		        query = query.Where(x => x.idOrganizacao == idOrganizacao);
		    }

    	    return query;
		}

		/// <summary>
        /// Salvar e remover os registros anteriores.
        /// </summary>
		public bool salvar(PaginaEstatuto OPaginaEstatuto) {

            OPaginaEstatuto.setDefaultInsertValues();

			db.PaginaEstatuto.Add(OPaginaEstatuto);

			db.SaveChanges();

		    bool flagSucesso = OPaginaEstatuto.id > 0;

		    int? idOrganizacao = OPaginaEstatuto.idOrganizacao;

		    if (flagSucesso) {

		        db.PaginaEstatuto
                  .Where(x => x.dtExclusao == null && x.idOrganizacao == idOrganizacao && x.id != OPaginaEstatuto.id)
                  .Update(x => new PaginaEstatuto { dtExclusao = DateTime.Now, idUsuarioExclusao = User.id() });
		    }

			return (OPaginaEstatuto.id > 0);

		}
	}
}