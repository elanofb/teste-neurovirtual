using System;
using System.Collections.Generic;
using System.Linq;
using DAL.Permissao;
using BLL.Services;
using System.Data.Entity;
using EntityFramework.BulkInsert.Extensions;
using EntityFramework.Extensions;

namespace BLL.Permissao {

	public class AcessoPermissaoBL : DefaultBL, IAcessoPermissaoBL {

		//Atributos
		private AcessoRecursoAcaoBL _AcessoRecursoAcaoBL;

		//Propriedades
		private AcessoRecursoAcaoBL OAcessoRecursoAcaoBL { get { return ( this._AcessoRecursoAcaoBL = this._AcessoRecursoAcaoBL ?? new AcessoRecursoAcaoBL());} }

		//Construtor
		public AcessoPermissaoBL(){
		}

		//Carregar 
		public AcessoPermissao carregar(int id){

			var query = from Perm in db.AcessoPermissao
						where	
							Perm.id == id &&
							Perm.flagExcluido == "N" 
						select	
							Perm;

			return query.FirstOrDefault();
		}

		//Carregar 
		public IQueryable<AcessoPermissao> listar(int idPerfil, int idRecurso, int idRecursoAcao){

			var query = from Perm in db.AcessoPermissao
						where	
							Perm.flagExcluido == "N" 
						select	
							Perm;

			if (idPerfil > 0) {

				query = query.Where(x => x.idPerfilAcesso == idPerfil);
				
			}

			if (idRecurso > 0) { 
				query = query.Where(x => x.idRecurso == idRecurso);
			}

			if (idRecursoAcao > 0) { 
				query = query.Where(x => x.idRecursoAcao == idRecursoAcao);
			}

			return query.AsNoTracking();
		}

		//Listagem dos recursos do sistema
		public IQueryable<RecursoSistemaVW> listarRecursos() {
			
			var query = from RS in db.RecursoSistemaVW
						select RS;

			return query.AsNoTracking();
		}

		//Listagem de permissoes pelo perfil de acesso
		public IQueryable<RecursoPermissaoVW> listarPermissoes(int idPerfilAcesso, int idOrganizacaoParam) {

			var query = from RS in db.RecursoPermissaoVW
						select RS;

			if (idPerfilAcesso > 0) {

				query = query.Where(x => x.idPerfilAcesso == idPerfilAcesso);
			}

		    if (idOrganizacaoParam > 0){
		        
                query = query.Where(x => x.idOrganizacao == idOrganizacaoParam || x.idOrganizacao == null);

		    }
			
			return query.AsNoTracking();
		}

		//Remover todas as permissões anteriores
		//Salvar as permissões configuradas
		public void salvarPermissoesRecursos(int idPerfil, List<AcessoRecurso> listaRecursos) {
			
			this.excluir(idPerfil, 0); 

			var listaInsert = new List<AcessoPermissao>();
			
			foreach (AcessoRecurso ORecurso in listaRecursos) {

				AcessoPermissao OAcessoPermissao = new AcessoPermissao();
				
				OAcessoPermissao.idPerfilAcesso = idPerfil;
				
				OAcessoPermissao.idRecurso = ORecurso.id;
				
				OAcessoPermissao.idGrupo = ORecurso.idRecursoGrupo;
				
				OAcessoPermissao.flagExcluido = "N";

				OAcessoPermissao.setDefaultInsertValues();
				
				listaInsert.Add(OAcessoPermissao);
			}

			db.Configuration.ValidateOnSaveEnabled = false;
			
			db.Configuration.AutoDetectChangesEnabled = false;
			
			db.BulkInsert(listaInsert);
			
		}

		// Salvar as permissões configuradas para actions
		public void salvarPermissoesAcoes(int idPerfil, int idRecursoAcao) {

			var OAcessoPermissao = this.listar(idPerfil, 0, idRecursoAcao).FirstOrDefault();
			if (OAcessoPermissao != null) {
				this.excluir(OAcessoPermissao.id);
			}

			var Acao = OAcessoRecursoAcaoBL.carregar(idRecursoAcao);
			if (Acao == null) return;

			OAcessoPermissao = new AcessoPermissao();
			OAcessoPermissao.idPerfilAcesso = idPerfil;
			OAcessoPermissao.idRecurso = Acao.idRecursoPai;
			OAcessoPermissao.idGrupo = Acao.idRecursoGrupo;
			OAcessoPermissao.idRecursoAcao = idRecursoAcao;
			OAcessoPermissao.flagExcluido = "N";

			OAcessoPermissao.setDefaultInsertValues<AcessoPermissao>();
			db.AcessoPermissao.Add(OAcessoPermissao);
			db.SaveChanges();
		}


		//Exclusao Lógica
		public UtilRetorno excluir(int id) {
			AcessoPermissao OAcessoPermissao = this.carregar(id);

			if(OAcessoPermissao == null){
				return UtilRetorno.newInstance(true, "O registro informado não foi localizado.");
			}

			OAcessoPermissao.flagExcluido = "S";
			OAcessoPermissao.dtAlteracao = DateTime.Now;
			this.db.SaveChanges();

			return UtilRetorno.newInstance(false, "O registro foi removido com sucesso");
		}

		//Exclusao Lógica
		public UtilRetorno excluir(int idPerfil, int idRecursoAcao) {
			
			var query = from Perm in db.AcessoPermissao
						where	
							Perm.flagExcluido == "N"
						select 
							Perm;
			
			if (idPerfil > 0) { 
				query = query.Where(x => x.idPerfilAcesso == idPerfil);
			}
			
			if (idRecursoAcao > 0) { 
				query = query.Where(x => x.idRecursoAcao == idRecursoAcao);
			}

			query.Update(x => new AcessoPermissao { flagExcluido = "S", dtAlteracao = DateTime.Now } );

			return UtilRetorno.newInstance(false, "Os registros foram removidos com sucesso");
		}
	}
}