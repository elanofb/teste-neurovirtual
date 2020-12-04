using System;
using System.Linq;
using BLL.Services;
using DAL.Relacionamentos;
using DAL.Repository.Base;
using EntityFramework.Extensions;

namespace BLL.Relacionamentos {

	public class OcorrenciaRelacionamentoPadraoBL : DefaultBL, IOcorrenciaRelacionamentoPadraoBL {

		//
		public OcorrenciaRelacionamentoPadraoBL() {
		}

        //Carregamento de registro único pelo ID
		public OcorrenciaRelacionamentoPadrao carregar(int id) {
			
			var query = from OcRel in db.OcorrenciaRelacionamentoPadrao
						where 
							OcRel.id == id && 
							OcRel.flagExcluido == "N"
						select OcRel;

			return query.FirstOrDefault();
		}

		//Listagem de Registros
		public IQueryable<OcorrenciaRelacionamentoPadrao> listar(string valorBusca, string ativo, string flagSistema = "N") {
			
			var query = from Oco in db.OcorrenciaRelacionamentoPadrao
						where Oco.flagExcluido == "N"
						select Oco;

			if (!String.IsNullOrEmpty(valorBusca)) {
				query = query.Where(x => x.descricao.Contains(valorBusca));
			}

			if (!String.IsNullOrEmpty(ativo)) {
				query = query.Where(x => x.ativo == ativo);
			}

            if (!String.IsNullOrEmpty(flagSistema)) {
				query = query.Where(x => x.flagSistema == flagSistema);
			}

			return query;
		}

		
        //Verificar se já existe um registro com a descrição informada, no entanto, que possua id diferente do informado
		public bool existe(string descricao, int id) {
			
			var query = from Oco in db.OcorrenciaRelacionamentoPadrao
						where Oco.descricao == descricao && Oco.id != id && Oco.flagExcluido == "N"
						select Oco;
			var ORegistro = query.Take(1).FirstOrDefault();
			return (ORegistro == null ? false : true);
		}

		
		//Realizar os tratamentos necessários
		//Salvar um novo registro
		public bool salvar(OcorrenciaRelacionamentoPadrao OOcorrenciaRelacionamento) {
			if(OOcorrenciaRelacionamento.id == 0){	
				return this.inserir(OOcorrenciaRelacionamento);
			}

			return this.atualizar(OOcorrenciaRelacionamento);
		}

        //Persistir e inserir um novo registro 
		//Inserir Instituicao
        private bool inserir(OcorrenciaRelacionamentoPadrao OOcorrenciaRelacionamento) { 
			
			OOcorrenciaRelacionamento.setDefaultInsertValues<OcorrenciaRelacionamentoPadrao>();
			
			db.OcorrenciaRelacionamentoPadrao.Add(OOcorrenciaRelacionamento);
			db.SaveChanges();

			return OOcorrenciaRelacionamento.id > 0;
		}

        //Persistir e atualizar um registro existente 
		//Atualizar dados da Instituicao
		private bool atualizar(OcorrenciaRelacionamentoPadrao OOcorrenciaRelacionamento) { 

			//Localizar existentes no banco
			OcorrenciaRelacionamentoPadrao dbOcorrenciaRelacionamento = this.carregar(OOcorrenciaRelacionamento.id);

			//Configurar valores padrão
			OOcorrenciaRelacionamento.setDefaultUpdateValues<OcorrenciaRelacionamentoPadrao>();

			//Atualizacao da Instituicao
			var InstituicaoEntry = db.Entry(dbOcorrenciaRelacionamento);
			InstituicaoEntry.CurrentValues.SetValues(OOcorrenciaRelacionamento);
			InstituicaoEntry.ignoreFields<OcorrenciaRelacionamentoPadrao>();

			db.SaveChanges();
			return OOcorrenciaRelacionamento.id > 0;
		}

        // Excluir Registro
		public bool excluir(int id) {
			
			db.OcorrenciaRelacionamentoPadrao.Where(x => x.id == id)
			  .Update(x => new OcorrenciaRelacionamentoPadrao { flagExcluido = "S", dtAlteracao = DateTime.Now });

			var listaCheck = db.OcorrenciaRelacionamentoPadrao.Where(x => x.id == id && x.flagExcluido == "N").ToList();
			return (listaCheck.Count == 0);
		}
	}
}