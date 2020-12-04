using System;
using System.Linq;
using DAL.Publicacoes;
using EntityFramework.Extensions;
using System.Json;
using BLL.Services;
using UTIL.Resources;
using System.Data.Entity;

namespace BLL.Publicacoes {

	public class CategoriaNoticiaBL : DefaultBL, ICategoriaNoticiaBL {
        
        //Construtor
		public CategoriaNoticiaBL() {
        }
		
        //
		public IQueryable<CategoriaNoticia> listar(string valorBusca = "", bool? ativo = null) {
			
			var query = from N in db.CategoriaNoticia
                        where N.flagExcluido == false
						select N;

            query = query.condicoesSeguranca();
            
			if (!String.IsNullOrEmpty(valorBusca)) {
				query = query.Where(x => x.descricao.Contains(valorBusca));
			}

			if (ativo != null) {
				query = query.Where(x => x.ativo == ativo);
			}

			return query;
        }

        //Listagem de categorias de noticias de acordo com o idTipoNoticia e se tiver alguma noticia cadastrada com esse tipo
        public IQueryable<CategoriaNoticia> listarPorTipoNoticia(int idTipoNoticia, bool? ativo = null) {

            var query = from N in db.CategoriaNoticia
                        where N.flagExcluido == false
                        select N;

            query = query.condicoesSeguranca();

            if (idTipoNoticia > 0) {
                query = query.Where(x => x.listaNoticias.Where(y => y.idTipoNoticia == idTipoNoticia).Any());
            }

            if (ativo != null) {
                query = query.Where(x => x.ativo == ativo);
            }

            return query;
        }

        //
        public CategoriaNoticia carregar(int id) {
			
			var query = db.CategoriaNoticia.Where(x => x.id == id);

            query = query.condicoesSeguranca();

			return query.FirstOrDefault();
		}
        
		//
		public bool salvar(CategoriaNoticia OCategoriaNoticia) {

		    OCategoriaNoticia.chaveUrl = UtilString.cleanAccents(OCategoriaNoticia.descricao).ToLower().Replace(" ", "-");

            if (OCategoriaNoticia.id == 0){	
				return this.inserir(OCategoriaNoticia);
			}

			return this.atualizar(OCategoriaNoticia);

		}

        //Persistir e inserir um novo registro 
        protected virtual bool inserir(CategoriaNoticia OCategoriaNoticia) {

            OCategoriaNoticia.setDefaultInsertValues();
			
			db.CategoriaNoticia.Add(OCategoriaNoticia);
			db.SaveChanges();

			return OCategoriaNoticia.id > 0;
		}

        //Persistir e atualizar um registro existente 
		protected virtual bool atualizar(CategoriaNoticia OCategoriaNoticia) {

            //Localizar existentes no banco
            CategoriaNoticia dbCategoriaNoticia = this.carregar(OCategoriaNoticia.id);

            if (dbCategoriaNoticia == null) {
                return false;
            }

            //Configurar valores padrão
            OCategoriaNoticia.setDefaultUpdateValues();

			//Atualizacao da Instituicao
			var CategoriaNoticiaEntry = db.Entry(dbCategoriaNoticia);
            CategoriaNoticiaEntry.CurrentValues.SetValues(OCategoriaNoticia);
            CategoriaNoticiaEntry.ignoreFields();

			db.SaveChanges();
			return OCategoriaNoticia.id > 0;
		}

        //Verificar se já existe um registro para evitar duplicidades
        public bool existe(string descricao, int id, int idPortal, int idOrganizacao) {
            var query = (from T in db.CategoriaNoticia
                         where T.descricao == descricao && T.id != id && idPortal == T.idPortal && idOrganizacao == T.idOrganizacao && T.flagExcluido == false
                         select T).AsNoTracking();

            query = query.condicoesSeguranca();

            var OTipoTitulo = query.Take(1).FirstOrDefault();

            return (OTipoTitulo != null);
        }

        //Alteracao de status
        public JsonMessageStatus alterarStatus(int id) {
	        var retorno = new JsonMessageStatus();

	        var item = this.carregar(id);

	        if (item == null) {
		        retorno.error = true;
		        retorno.message = NotificationMessages.invalid_register_id;
	        } else {
		        item.ativo = (item.ativo == true ? false : true);
		        db.SaveChanges();
		        retorno.active = (item.ativo == true ? "S" : "N");
		        retorno.message = NotificationMessages.updateSuccess;
	        }
	        return retorno;
        }

		//
		public bool excluir(int id) {

			db.CategoriaNoticia.Where(x => x.id == id)
				.Update(x => new CategoriaNoticia { flagExcluido = true, dtAlteracao = DateTime.Now });

			var listaCheck = db.CategoriaNoticia.Where(x => x.id == id && x.flagExcluido == false).ToList();

			return (listaCheck.Count == 0);
		}
	}
}