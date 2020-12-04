using System;
using System.Json;
using System.Linq;
using BLL.Services;
using DAL.Associados;
using UTIL.Resources;

namespace BLL.Associados {

	public class MotivoDesligamentoBL : DefaultBL, IMotivoDesligamentoBL {

        //Atributos

        //Propriedades

		//
		public MotivoDesligamentoBL(){
		}

        //Carregamento de registro único pelo ID
		public MotivoDesligamento carregar(int id) {
			
			var query = from Item in db.MotivoDesligamento
						where 
							Item.id == id && 
							Item.flagExcluido == false
						select Item;

            query = query.condicoesSeguranca();

            return query.FirstOrDefault();
		}

		//
		public IQueryable<MotivoDesligamento> listar(string valorBusca, bool? ativo) {
			
			var query = from C in db.MotivoDesligamento
                        where C.flagExcluido == false
						select C;

            query = query.condicoesSeguranca();

			if (!String.IsNullOrEmpty(valorBusca)) {
				query = query.Where(x => x.descricao.Contains(valorBusca));
			}

			if (ativo != null) {
				query = query.Where(x => x.ativo == ativo);
			}

			return query;
		}

		//Verificar se já existe um registro com a descrição informada, no entanto, que possua id diferente do informado
		public bool existe(MotivoDesligamento OMotivoDesligamento, int id) {

			var query = from C in db.MotivoDesligamento
                        where 
                            C.descricao == OMotivoDesligamento.descricao && 
                            C.id != id && 
                            C.flagExcluido == false
						select C;

            query = query.condicoesSeguranca();

			return query.Any();
		}

		//Realizar os tratamentos necessários
		//Salvar um novo registro
		public bool salvar(MotivoDesligamento OMotivoDesligamento) {

			if(OMotivoDesligamento.id == 0){	
				return this.inserir(OMotivoDesligamento);
			}

			return this.atualizar(OMotivoDesligamento);
		}

        //Persistir e inserir um novo registro 
        //Inserir MotivoDesligamento
        private bool inserir(MotivoDesligamento OMotivoDesligamento) {

            OMotivoDesligamento.setDefaultInsertValues<MotivoDesligamento>();

			db.MotivoDesligamento.Add(OMotivoDesligamento);

			db.SaveChanges();

			return OMotivoDesligamento.id > 0;
		}

        //Persistir e atualizar um registro existente 
        //Atualizar dados da MotivoDesligamento
        private bool atualizar(MotivoDesligamento OMotivoDesligamento) {

            //Localizar existentes no banco
            MotivoDesligamento dbMotivoDesligamento = this.carregar(OMotivoDesligamento.id);

            if(dbMotivoDesligamento == null) {
                return false;
            }

            //Configurar valores padrão
            OMotivoDesligamento.setDefaultUpdateValues();

            //Atualizacao da MotivoDesligamento
            var MotivoDesligamentoEntry = db.Entry(dbMotivoDesligamento);
            MotivoDesligamentoEntry.CurrentValues.SetValues(OMotivoDesligamento);
            MotivoDesligamentoEntry.ignoreFields();

			db.SaveChanges();

			return OMotivoDesligamento.id > 0;
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