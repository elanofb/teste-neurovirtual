using System;
using System.Json;
using System.Linq;
using BLL.Services;
using DAL.RamosAtividade;
using UTIL.Resources;

namespace BLL.RamosAtividade {

	public class RamoAtividadeBL : DefaultBL, IRamoAtividadeBL {

        //Atributos

        //Propriedades

		//
		public RamoAtividadeBL() {
		}

        //Carregamento de registro único pelo ID
		public RamoAtividade carregar(int id) {
			
			var query = from Item in db.RamoAtividade
						where 
							Item.id == id && 
							Item.flagExcluido == false
						select Item;

            query = query.condicoesSeguranca();

			return query.FirstOrDefault();

		}

		// Listagem de Registros
		public IQueryable<RamoAtividade> listar(string valorBusca, bool? ativo) {

			var query = from T in db.RamoAtividade
						where T.flagExcluido == false
						select T;
            
            query = query.condicoesSeguranca();

			if (!String.IsNullOrEmpty(valorBusca)) {
				query = query.Where(x => x.descricao.Contains(valorBusca) );
			}

			if (ativo.HasValue) {
				query = query.Where(x => x.ativo == ativo);
			}

			return query;
		}

		
		// Verificar se já existe um registro com a descrição informada, no entanto, que possua id diferente do informado
		public bool existe(RamoAtividade ORamoAtividade, int id) {
			
			var query = from T in db.RamoAtividade
						where T.descricao == ORamoAtividade.descricao && T.id != id && T.flagExcluido == false
						select T;

            query = query.condicoesSeguranca();

			var OItem = query.Take(1).FirstOrDefault();

            return (OItem != null);
		}

		//Realizar os tratamentos necessários
		//Salvar um novo registro
		public bool salvar(RamoAtividade ORamoAtividade) {

			if(ORamoAtividade.id == 0){	
				return this.inserir(ORamoAtividade);
			}

			return this.atualizar(ORamoAtividade);
		}

        //Persistir e inserir um novo registro 
		//Inserir RamoAtividade
        private bool inserir(RamoAtividade ORamoAtividade) { 

			ORamoAtividade.setDefaultInsertValues<RamoAtividade>();

            ORamoAtividade.flagSistema = false;

			db.RamoAtividade.Add(ORamoAtividade);

			db.SaveChanges();

			return ORamoAtividade.id > 0;
		}

        //Persistir e atualizar um registro existente 
		//Atualizar dados da RamoAtividade
		private bool atualizar(RamoAtividade ORamoAtividade) { 

			//Localizar existentes no banco
			RamoAtividade dbRamoAtividade = this.carregar(ORamoAtividade.id);

            if(dbRamoAtividade == null) {
                return false;
            }

			//Configurar valores padrão
			ORamoAtividade.setDefaultUpdateValues();

			//Atualizacao da RamoAtividade
			var RamoAtividadeEntry = db.Entry(dbRamoAtividade);
			RamoAtividadeEntry.CurrentValues.SetValues(ORamoAtividade);
			RamoAtividadeEntry.ignoreFields();

			db.SaveChanges();

			return ORamoAtividade.id > 0;
		}

        //Alteracao de status
        public JsonMessageStatus alterarStatus(int id) {
			var retorno = new JsonMessageStatus();

			var item = this.carregar(id);

			if (item == null) {
				retorno.error = true;
				retorno.message = NotificationMessages.invalid_register_id;
			} else {
				item.ativo = (item.ativo != true);
				db.SaveChanges();
				retorno.active = item.ativo == true ? "S" : "N";
				retorno.message = NotificationMessages.updateSuccess;
			}
			return retorno;
		}

		// Excluir Registro
		public UtilRetorno excluir(int id, int idUsuarioExclusao) {

		    var ORegistro = this.carregar(id);

		    if (ORegistro == null) {
		        return UtilRetorno.newInstance(true, "O registro informado não pôde ser localizado.");
		    }

		    ORegistro.flagExcluido = true;

            ORegistro.idUsuarioAlteracao = idUsuarioExclusao;

            ORegistro.dtAlteracao = DateTime.Now;

            db.SaveChanges();

		    return UtilRetorno.newInstance(false, "Os dados foram atualizados com sucesso.");
		}
	}
}