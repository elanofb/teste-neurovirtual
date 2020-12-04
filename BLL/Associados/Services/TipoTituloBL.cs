using System;
using System.Linq;
using System.Data.Entity;
using System.Json;
using DAL.Associados;
using BLL.Services;
using DAL.Permissao.Security.Extensions;
using DAL.Repository.Base;
using EntityFramework.Extensions;
using UTIL.Resources;

namespace BLL.Associados {

	public class TipoTituloBL : DefaultBL, ITipoTituloBL {

		//
		public TipoTituloBL(){
		}

		//Carregamento de registro pelo ID
		public TipoTitulo carregar(int id) { 
			var query = (from Ti in db.TipoTitulo
						 where 
							Ti.flagExcluido == "N" &&
							Ti.id == id
						 select Ti
						);

            query = query.condicoesSeguranca();

			return query.FirstOrDefault();
		}

		//listar registros do banco com base nos parametros
		public IQueryable<TipoTitulo> listar(int idInstituicao, string valorBusca, string ativo) {
			var query = from T in db.TipoTitulo
									.Include(x => x.Instituicao)
						where T.flagExcluido == "N"
						select T;

            query = query.condicoesSeguranca();

			if (idInstituicao > 0) {
				query = query.Where(x => x.idInstituicao == idInstituicao);
			}

			if (!String.IsNullOrEmpty(valorBusca)) {
				query = query.Where(x => x.descricao.Contains(valorBusca));
			}

			if (!String.IsNullOrEmpty(ativo)) {
				query = query.Where(x => x.ativo == ativo);
			}

			return query;
		}


		//Verificar se deve-se atualizar um registro existente ou criar um novo
		public bool salvar(TipoTitulo OTipoTitulo) {

			if (OTipoTitulo.id == 0) { 
				return this.inserir(OTipoTitulo);
			}

			return this.atualizar(OTipoTitulo);
		}

		//Persistir o objecto e salvar na base de dados
		private bool inserir(TipoTitulo OTipoTitulo) { 

			OTipoTitulo.setDefaultInsertValues<TipoTitulo>();
			db.TipoTitulo.Add(OTipoTitulo);
			db.SaveChanges();

			return (OTipoTitulo.id > 0);
		}

		//Persistir o objecto e atualizar informações
		private bool atualizar(TipoTitulo OTipoTitulo) { 
			OTipoTitulo.setDefaultUpdateValues<TipoTitulo>();

			//Localizar existentes no banco
			TipoTitulo dbTipoTitulo = this.carregar(OTipoTitulo.id);		

            if (dbTipoTitulo == null) {
                return false;
            }

			var TipoEntry = db.Entry(dbTipoTitulo);
			TipoEntry.CurrentValues.SetValues(OTipoTitulo);
			TipoEntry.ignoreFields<TipoTitulo>();

			db.SaveChanges();
			return (OTipoTitulo.id > 0);
		}

	
		//Verificar se já existe um registro para evitar duplicidades
		public bool existe(string descricao, int id) {
			var query = (from T in db.TipoTitulo
						where T.descricao == descricao && T.id != id && T.flagExcluido == "N"
						select T).AsNoTracking();

            query = query.condicoesSeguranca();

			var OTipoTitulo = query.Take(1).FirstOrDefault();
			return (OTipoTitulo == null ? false : true);
		}		
		
        //Alteracao de status
        public JsonMessageStatus alterarStatus(int id) {
	        var retorno = new JsonMessageStatus();

	        var item = this.carregar(id);

	        if (item == null) {
		        retorno.error = true;
		        retorno.message = NotificationMessages.invalid_register_id;
	        } else {
		        item.ativo = (item.ativo == "S" ? "N" : "S");
		        db.SaveChanges();
		        retorno.active = item.ativo;
		        retorno.message = NotificationMessages.updateSuccess;
	        }
	        return retorno;
        }

		//Exclusao logica de registro
		public UtilRetorno excluir(int id) {
			UtilRetorno Retorno = UtilRetorno.getInstance();
			Retorno.flagError = false;

		    var idUsuario = User.id();

            this.db.TipoTitulo
						.Where(x => x.id == id)
						.Update(x => new TipoTitulo{ flagExcluido = "S", idUsuarioAlteracao = idUsuario, dtAlteracao = DateTime.Now });

			return Retorno;
		}
	}
}