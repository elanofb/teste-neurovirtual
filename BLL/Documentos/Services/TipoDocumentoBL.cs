using System;
using System.Linq;
using DAL.Documentos;
using EntityFramework.Extensions;
using System.Data.Entity;
using BLL.Services;
using DAL.Permissao.Security.Extensions;

namespace BLL.Documentos {

	public class TipoDocumentoBL : DefaultBL, ITipoDocumentoBL {


		//
		public TipoDocumento carregar(int id) {

			var query = from DocPes in db.TipoDocumento.Include("CategoriaDocumento")
						where DocPes.id == id && DocPes.flagExcluido == "N"
						select DocPes;
			TipoDocumento OTipoDocumento = query.FirstOrDefault();
			return OTipoDocumento;
		}

		//Listagem de registros de acordo com parametros informados
		public IQueryable<TipoDocumento> listar(int idCategoriaDocumento, string descricao, string ativo = "S") {

			var query = from TipoDoc in db.TipoDocumento
										.Include(x => x.CategoriaDocumento)
						where TipoDoc.flagExcluido == "N"
						select TipoDoc;

			query = query.Where(x => x.idCategoriaDocumento == idCategoriaDocumento);

			if (!String.IsNullOrEmpty(descricao)) {
				query = query.Where(x => x.descricao.Contains(descricao));
			}

			if (!String.IsNullOrEmpty(ativo)) {
				query = query.Where(x => x.ativo == ativo);
			}

			return query;
		}

        public bool salvar(TipoDocumento OTipoDocumento) {
            
            if (OTipoDocumento.id == 0) {
                return this.inserir(OTipoDocumento);
            }

            return this.atualizar(OTipoDocumento);
        }

        //Persistir o objecto e salvar na base de dados
        private bool inserir(TipoDocumento OTipoDocumento) {

            OTipoDocumento.setDefaultInsertValues<TipoDocumento>();
            db.TipoDocumento.Add(OTipoDocumento);
            db.SaveChanges();

            return (OTipoDocumento.id > 0);
        }

        //Persistir o objecto e atualizar informações
        private bool atualizar(TipoDocumento OTipoDocumento) {

            OTipoDocumento.setDefaultUpdateValues<TipoDocumento>();

            //Localizar existentes no BoletoContaEmissao
            TipoDocumento dbTipoProduto = this.carregar(OTipoDocumento.id);
            var TipoEntry = db.Entry(dbTipoProduto);
            TipoEntry.CurrentValues.SetValues(OTipoDocumento);
            TipoEntry.ignoreFields<TipoDocumento>();

            db.SaveChanges();
            return (OTipoDocumento.id > 0);
        }

        //Exclusao do registro de forma lógica - não física
        public bool excluir(int id) {
		    var idUsuario = User.id();

            db.TipoDocumento.Where(x => x.id == id)
				.Update(x => new TipoDocumento { flagExcluido = "S", dtAlteracao = DateTime.Now, idUsuarioAlteracao = idUsuario });

			return true;
		}
	}
}