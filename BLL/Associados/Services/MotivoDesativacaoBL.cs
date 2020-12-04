using System;
using System.Json;
using System.Linq;
using BLL.Services;
using DAL.Associados;
using UTIL.Resources;

namespace BLL.Associados {

	public class MotivoDesativacaoBL : DefaultBL, IMotivoDesativacaoBL {

        //Atributos

        //Propriedades

		//
		public MotivoDesativacaoBL(){
		}

        //Carregamento de registro único pelo ID
		public MotivoDesativacao carregar(int id) {
			
			var query = from Item in db.MotivoDesativacao
						where 
							Item.id == id && 
							Item.flagExcluido == false
						select Item;

            query = query.condicoesSeguranca();

            return query.FirstOrDefault();
		}

		//
		public IQueryable<MotivoDesativacao> listar(string valorBusca, bool? ativo) {
			
			var query = from C in db.MotivoDesativacao
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
		public bool existe(MotivoDesativacao OMotivoDesativacao, int id) {

			var query = from C in db.MotivoDesativacao
                        where 
                            C.descricao == OMotivoDesativacao.descricao && 
                            C.id != id && 
                            C.flagExcluido == false
						select C;

            query = query.condicoesSeguranca();

			return query.Any();
		}

		//Realizar os tratamentos necessários
		//Salvar um novo registro
		public bool salvar(MotivoDesativacao OMotivoDesativacao) {

			if(OMotivoDesativacao.id == 0){	
				return this.inserir(OMotivoDesativacao);
			}

			return this.atualizar(OMotivoDesativacao);
		}

        //Persistir e inserir um novo registro 
        //Inserir MotivoDesativacao
        private bool inserir(MotivoDesativacao OMotivoDesativacao) {

            OMotivoDesativacao.setDefaultInsertValues<MotivoDesativacao>();

			db.MotivoDesativacao.Add(OMotivoDesativacao);

			db.SaveChanges();

			return OMotivoDesativacao.id > 0;
		}

        //Persistir e atualizar um registro existente 
        //Atualizar dados da MotivoDesativacao
        private bool atualizar(MotivoDesativacao OMotivoDesativacao) {

            //Localizar existentes no banco
            MotivoDesativacao dbMotivoDesativacao = this.carregar(OMotivoDesativacao.id);

            if(dbMotivoDesativacao == null) {
                return false;
            }

            //Configurar valores padrão
            OMotivoDesativacao.setDefaultUpdateValues();

            //Atualizacao da MotivoDesativacao
            var MotivoDesativacaoEntry = db.Entry(dbMotivoDesativacao);
            MotivoDesativacaoEntry.CurrentValues.SetValues(OMotivoDesativacao);
            MotivoDesativacaoEntry.ignoreFields();

			db.SaveChanges();

			return OMotivoDesativacao.id > 0;
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