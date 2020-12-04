using System;
using System.Data.Entity;
using System.Linq;
using BLL.Services;
using DAL.Permissao;
using DAL.Repository.Base;

namespace BLL.Permissao {

	public class AcessoRecursoGrupoBL : DefaultBL, IAcessoRecursoGrupoBL {

		//Atributos

		//Propriedades

		//Construtor
		public AcessoRecursoGrupoBL(){
		}

        //
	    public AcessoRecursoGrupo carregar(int id) {

	        var query = from gr in db.AcessoRecursoGrupo
	            where
	                gr.id == id
	            select gr;

	        return query.FirstOrDefault();
	    }

		//Listar grupos conforme parametros informados
		public IQueryable<AcessoRecursoGrupo> listar(string ativo) {
			
			var query = from Grupo in db.AcessoRecursoGrupo
						where	
							Grupo.flagExcluido == "N"
						select 
							Grupo;

			if (!String.IsNullOrEmpty(ativo)) {
				query = query.Where(x => x.ativo == ativo);
			}

			return query.AsNoTracking();
		}

        //Verificar se deve-se atualizar um registro existente ou criar um novo
		public AcessoRecursoGrupo salvar(AcessoRecursoGrupo OAcessoRecursoGrupo) {

			if (OAcessoRecursoGrupo.id == 0) { 
				return this.inserir(OAcessoRecursoGrupo);
			}

			return this.atualizar(OAcessoRecursoGrupo);

		}

		//Persistir o objecto e salvar na base de dados
		private AcessoRecursoGrupo inserir(AcessoRecursoGrupo OAcessoRecursoGrupo) {

            OAcessoRecursoGrupo.setDefaultInsertValues();

            db.AcessoRecursoGrupo.Add(OAcessoRecursoGrupo);

			db.SaveChanges();

			return OAcessoRecursoGrupo;
		}

		//Persistir o objecto e atualizar informações
		private AcessoRecursoGrupo atualizar(AcessoRecursoGrupo OAcessoRecursoGrupo) { 

			OAcessoRecursoGrupo.setDefaultUpdateValues();

			//Localizar existentes no banco
			AcessoRecursoGrupo dbItem = this.carregar(OAcessoRecursoGrupo.id);		

			var ItemEntry = db.Entry(dbItem);

            ItemEntry.CurrentValues.SetValues(OAcessoRecursoGrupo);

            ItemEntry.ignoreFields();

			db.SaveChanges();

            return OAcessoRecursoGrupo;
		}
	}
}