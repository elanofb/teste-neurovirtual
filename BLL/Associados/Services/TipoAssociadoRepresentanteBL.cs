using System;
using System.Linq;
using System.Data.Entity;
using DAL.Associados;
using BLL.Services;
using DAL.Permissao.Security.Extensions;
using EntityFramework.Extensions;

namespace BLL.Associados {

	public class TipoAssociadoRepresentanteBL : DefaultBL, ITipoAssociadoRepresentanteBL {

		//
		public TipoAssociadoRepresentanteBL(){
		}

		//Carregamento de registro pelo ID
		public TipoAssociadoRepresentante carregar(int id) { 
			var query = (from Ti in db.TipoAssociadoRepresentante
						 where 
							Ti.flagExcluido == "N" &&
							Ti.id == id
						 select Ti
						);

			return query.FirstOrDefault();
		}

		//Carregamento de registro pelo ID
		public TipoAssociadoRepresentante carregarPorDescricao(string descricao, int idCategoria) { 
			var query = (from E in db.TipoAssociadoRepresentante
						 where 
							E.flagExcluido == "N" &&
							E.descricao == descricao 
						 select E
						);

			return query.FirstOrDefault();
		}

		//listar registros do banco com base nos parametros
		public IQueryable<TipoAssociadoRepresentante> listar(string valorBusca, bool? flagIsento, string ativo) {

			var query = from T in db.TipoAssociadoRepresentante
						where T.flagExcluido == "N"
						select T;

			if (!String.IsNullOrEmpty(valorBusca)) {
				query = query.Where(x => x.descricao.Contains(valorBusca));
			}

			if (!String.IsNullOrEmpty(ativo)) {
				query = query.Where(x => x.ativo == ativo);
			}

			return query;
		}


		//Verificar se deve-se atualizar um registro existente ou criar um novo
		public bool salvar(TipoAssociadoRepresentante OTipoAssociadoRepresentante) {

			if (OTipoAssociadoRepresentante.id == 0) { 
				return this.inserir(OTipoAssociadoRepresentante);
			}

			return this.atualizar(OTipoAssociadoRepresentante);
		}

		//Persistir o objecto e salvar na base de dados
		private bool inserir(TipoAssociadoRepresentante OTipoAssociadoRepresentante) { 

			OTipoAssociadoRepresentante.setDefaultInsertValues<TipoAssociadoRepresentante>();
			db.TipoAssociadoRepresentante.Add(OTipoAssociadoRepresentante);
			db.SaveChanges();

			return (OTipoAssociadoRepresentante.id > 0);
		}

		//Persistir o objecto e atualizar informações
		private bool atualizar(TipoAssociadoRepresentante OTipoAssociadoRepresentante) { 
			OTipoAssociadoRepresentante.setDefaultUpdateValues<TipoAssociadoRepresentante>();

			//Localizar existentes no banco
			TipoAssociadoRepresentante dbTipoAssociadoRepresentante = this.carregar(OTipoAssociadoRepresentante.id);		
			var TipoEntry = db.Entry(dbTipoAssociadoRepresentante);
			TipoEntry.CurrentValues.SetValues(OTipoAssociadoRepresentante);
			TipoEntry.ignoreFields(new[]{"flagSistema"});

			db.SaveChanges();
			return (OTipoAssociadoRepresentante.id > 0);
		}

	
		//Verificar se já existe um registro para evitar duplicidades
		public bool existe(string descricao, int idCategoria, int id) {
			var query = (from T in db.TipoAssociadoRepresentante
						where T.descricao == descricao && T.id != id && T.flagExcluido == "N" 
						select T).AsNoTracking();

			var OTipoTitulo = query.Take(1).FirstOrDefault();
			return (OTipoTitulo != null);
		}		
		
		//Exclusao logica de registro
		public UtilRetorno excluir(int id) {
			UtilRetorno Retorno = UtilRetorno.getInstance();
			Retorno.flagError = false;

		    var idUsuario = User.id();

            this.db.TipoAssociadoRepresentante
						.Where(x => x.id == id)
						.Update(x => new TipoAssociadoRepresentante{ flagExcluido = "S", idUsuarioAlteracao = idUsuario, dtAlteracao = DateTime.Now });

			return Retorno;
		}
	}
}