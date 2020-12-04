using System;
using System.Linq;
using System.Data.Entity;
using BLL.Services;
using DAL.AreasAtuacao;
using DAL.Permissao.Security.Extensions;
using EntityFramework.Extensions;

namespace BLL.AreasAtuacao {

	public class AreaAtuacaoTipoAssociadoBL : DefaultBL {

		//Carregamento de registro pelo ID
		public AreaAtuacaoTipoAssociado carregar(int id) { 

			var query = (from Ti in db.AreaAtuacaoTipoAssociado.Include(x => x.TipoAssociado).Include(x => x.AreaAtuacao)
						 where 
							Ti.flagExcluido == "N" &&
							Ti.id == id
						 select Ti
						);

			return query.FirstOrDefault();
		}

		//Carregamento de registro pelo ID
		public AreaAtuacaoTipoAssociado carregarPorDescricao(string descricao, int idCategoria) { 
			var query = (from E in db.AreaAtuacaoTipoAssociado.Include(x => x.TipoAssociado).Include(x => x.AreaAtuacao)
						 where 
							E.flagExcluido == "N" &&
							E.descricao == descricao 
						 select E
						);

			return query.FirstOrDefault();
		}

		//listar registros do banco com base nos parametros
		public IQueryable<AreaAtuacaoTipoAssociado> listar(int idTipoAssociado, string ativo) {
		
			var query = from T in db.AreaAtuacaoTipoAssociado.Include(x => x.TipoAssociado).Include(x => x.AreaAtuacao)
						where T.flagExcluido == "N"
						select T;

			query = query.Where(x => x.idTipoAssociado == idTipoAssociado);

			if (!String.IsNullOrEmpty(ativo)) {
				query = query.Where(x => x.ativo == ativo);
			}

			return query;
		}


		//Verificar se deve-se atualizar um registro existente ou criar um novo
		public bool salvar(AreaAtuacaoTipoAssociado OAreaAtuacaoTipoAssociado) {

			if (OAreaAtuacaoTipoAssociado.id == 0) { 
				return this.inserir(OAreaAtuacaoTipoAssociado);
			}

			return this.atualizar(OAreaAtuacaoTipoAssociado);
		}

		//Persistir o objecto e salvar na base de dados
		private bool inserir(AreaAtuacaoTipoAssociado OAreaAtuacaoTipoAssociado) { 

			OAreaAtuacaoTipoAssociado.setDefaultInsertValues();
			db.AreaAtuacaoTipoAssociado.Add(OAreaAtuacaoTipoAssociado);
			db.SaveChanges();

			return (OAreaAtuacaoTipoAssociado.id > 0);
		}

		//Persistir o objecto e atualizar informações
		private bool atualizar(AreaAtuacaoTipoAssociado OAreaAtuacaoTipoAssociado) { 
			OAreaAtuacaoTipoAssociado.setDefaultUpdateValues();

			//Localizar existentes no banco
			AreaAtuacaoTipoAssociado dbAreaAtuacaoTipoAssociado = this.carregar(OAreaAtuacaoTipoAssociado.id);		
			var TipoEntry = db.Entry(dbAreaAtuacaoTipoAssociado);
			TipoEntry.CurrentValues.SetValues(OAreaAtuacaoTipoAssociado);
			TipoEntry.ignoreFields(new[]{"flagSistema"});

			db.SaveChanges();
			return (OAreaAtuacaoTipoAssociado.id > 0);
		}

	
		//Verificar se já existe um registro para evitar duplicidades
		public bool existe(string descricao, int idCategoria, int id) {
			var query = (from T in db.AreaAtuacaoTipoAssociado
						where T.descricao == descricao && T.id != id && T.flagExcluido == "N" 
						select T).AsNoTracking();

			var OTipoTitulo = query.Take(1).FirstOrDefault();
			return (OTipoTitulo == null ? false : true);
		}		
		
		//Exclusao logica de registro
		public UtilRetorno excluir(int id) {
			UtilRetorno Retorno = UtilRetorno.getInstance();
			Retorno.flagError = false;

		    int idUsuario = User.id();

			this.db.AreaAtuacaoTipoAssociado
						.Where(x => x.id == id)
						.Update(x => new AreaAtuacaoTipoAssociado{ flagExcluido = "S", idUsuarioAlteracao = idUsuario, dtAlteracao = DateTime.Now });

			return Retorno;
		}
	}
}