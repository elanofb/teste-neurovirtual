using System;
using System.Linq;
using DAL.Permissao;
using BLL.Services;
using System.Data.Entity;

namespace BLL.Permissao {

	public class AcessoRecursoAcaoBL : DefaultBL, IAcessoRecursoAcaoBL {

		//Atributos

		//Propriedades

		//Construtor
		public AcessoRecursoAcaoBL(){
		}

		//Carregar 
		public AcessoRecursoAcao carregar(int id){

			var query = from Acao in db.AcessoRecursoAcao
						where	
							Acao.id == id &&
							Acao.flagExcluido == "N" 
						select	
							Acao;

			return query.FirstOrDefault();
		}

		//Listagem a partir dos parametros informados
		public IQueryable<AcessoRecursoAcao> listar(int idRecursoGrupo, int idRecurso, string ativo) { 
		
			var query = from Acao in db.AcessoRecursoAcao
						where	
							Acao.flagExcluido == "N"
						select	
							Acao;

			if (idRecursoGrupo > 0) {
				query = query.Where(x => x.idRecursoGrupo == idRecursoGrupo);
			}

			if (idRecurso > 0) {
				query = query.Where(x => x.idRecursoPai == idRecurso);
			}

			if (!String.IsNullOrEmpty(ativo)) {
				query = query.Where(x => x.ativo == ativo);
			}

			return query.AsNoTracking();
				
		}

		//validar registro
		public UtilRetorno validar(AcessoRecursoAcao OAcessoRecursoAcao) {

			if (OAcessoRecursoAcao.idRecursoPai == 0 && OAcessoRecursoAcao.idRecursoGrupo == 0) {
				return UtilRetorno.newInstance(true, "O módulo ou menu não foi informado.");
			}

			if (String.IsNullOrEmpty(OAcessoRecursoAcao.descricao)) {
				return UtilRetorno.newInstance(true, "O descrição da ação não foi informada.");
			}

			if (String.IsNullOrEmpty(OAcessoRecursoAcao.controller)) {
				return UtilRetorno.newInstance(true, "O controlador da ação não foi informada.");
			}

			if (String.IsNullOrEmpty(OAcessoRecursoAcao.action)) {
				return UtilRetorno.newInstance(true, "O nome da ação não foi informada.");
			}

			return UtilRetorno.newInstance(false, "");
		}

		//Realizar os tratamentos necessarios
		//Salvar um novo registro
		//Verificar se deve-se atualizar um registro existente ou criar um novo
		public bool salvar(AcessoRecursoAcao OAcessoRecursoAcao) {
			bool flagSucesso = false;
			
			if (OAcessoRecursoAcao.id == 0) { 
				flagSucesso = this.inserir(OAcessoRecursoAcao);
			} else { 
				flagSucesso = this.atualizar(OAcessoRecursoAcao);
			}

			return flagSucesso;
		}

		//Persistir o objecto e salvar na base de dados
		private bool inserir(AcessoRecursoAcao OAcessoRecursoAcao) { 

			OAcessoRecursoAcao.setDefaultInsertValues<AcessoRecursoAcao>();
			db.AcessoRecursoAcao.Add(OAcessoRecursoAcao);
			db.SaveChanges();

			return (OAcessoRecursoAcao.id > 0);
		}

		//Persistir o objecto e atualizar informações
		private bool atualizar(AcessoRecursoAcao OAcessoRecursoAcao) { 
			OAcessoRecursoAcao.setDefaultUpdateValues<AcessoRecursoAcao>();

			//Localizar existentes no banco
			AcessoRecursoAcao dbAcessoRecursoAcao = this.carregar(OAcessoRecursoAcao.id);		
			var AcessoRecursoAcaoEntry = db.Entry(dbAcessoRecursoAcao);
			AcessoRecursoAcaoEntry.CurrentValues.SetValues(OAcessoRecursoAcao);
			AcessoRecursoAcaoEntry.ignoreFields<AcessoRecursoAcao>();

			db.SaveChanges();
			return (OAcessoRecursoAcao.id > 0);
		}

		//Exclusao logica
		public UtilRetorno excluir(int id) {

			AcessoRecursoAcao OAcessoRecursoAcao = this.carregar(id);

			if(OAcessoRecursoAcao == null){
				return UtilRetorno.newInstance(true, "O registro informado não foi localizado.");
			}

			OAcessoRecursoAcao.flagExcluido = "S";
			OAcessoRecursoAcao.dtAlteracao = DateTime.Now;
			this.db.SaveChanges();

			return UtilRetorno.newInstance(false, "O registro foi removido com sucesso.");
		}
	}
}