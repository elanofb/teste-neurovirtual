using System;
using System.Linq;
using DAL.Documentos;
using DAL.Repository.Base;
using EntityFramework.Extensions;

namespace BLL.Documentos {

	public class CategoriaDocumentoBL : TableRepository<CategoriaDocumento> {

		//
		public CategoriaDocumentoBL() {
		}

		//
		public CategoriaDocumento carregar(int id) {
			var db = this.getDataContext();
			var query = from CatDoc in db.CategoriaDocumento
						where CatDoc.id == id && CatDoc.flagExcluido == "N"
						select CatDoc;
			CategoriaDocumento OCategoriaDocumento = query.FirstOrDefault();
			return OCategoriaDocumento;
		}

		//
		public IQueryable<CategoriaDocumento> listar(string descricao, string ativo = "S") {
			var db = this.getDataContext();
			var query = from CatDoc in db.CategoriaDocumento
						where CatDoc.flagExcluido == "N"
						select CatDoc;

			if (!String.IsNullOrEmpty(descricao)) {
				query = query.Where(x => x.descricao.Contains(descricao));
			}

			if (!String.IsNullOrEmpty(ativo)) {
				query = query.Where(x => x.ativo == ativo);
			}

			return query;
		}

		/**
		 * Salvar Dados Principais
		 */

		public bool salvar(CategoriaDocumento OCategoriaDocumento) {
			this.save(OCategoriaDocumento, false);
			return (OCategoriaDocumento.id > 0);
		}

		/**
		 *
		 */

		public bool excluir(int[] ids) {
			this.getDataContext().CategoriaDocumento.Where(x => ids.Contains(x.id))
				.Update(x => new CategoriaDocumento { flagExcluido = "S" });

			var listaCheck = this.getDataContext().CategoriaDocumento.Where(x => ids.Contains(x.id) && x.flagExcluido == "N").ToList();
			return (listaCheck.Count == 0);
		}
	}
}