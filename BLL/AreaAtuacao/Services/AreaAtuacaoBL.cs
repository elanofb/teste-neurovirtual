using System;
using System.Collections.Generic;
using System.Json;
using System.Linq;
using BLL.Services;
using DAL.AreasAtuacao;
using DAL.Associados;
using UTIL.Resources;

namespace BLL.AreasAtuacao {

	public class AreaAtuacaoBL : DefaultBL, IAreaAtuacaoBL {

        //Atributos

        //Propriedades

		//
		public AreaAtuacaoBL(){
		}

        //Carregamento de registro único pelo ID
		public AreaAtuacao carregar(int id) {
			
			var query = from Item in db.AreaAtuacao
						where 
							Item.id == id && 
							Item.flagExcluido == "N"
						select Item;

            query = query.condicoesSeguranca();

			return query.FirstOrDefault();
		}

		//
		public IQueryable<AreaAtuacao> listar(string valorBusca, string ativo) {
			
			var query = from C in db.AreaAtuacao
						where C.flagExcluido == "N"
						select C;

            query = query.condicoesSeguranca();

			if (!String.IsNullOrEmpty(valorBusca)) {
				query = query.Where(x => x.descricao.Contains(valorBusca));
			}

			if (!String.IsNullOrEmpty(ativo)) {
				query = query.Where(x => x.ativo == ativo);
			}

			return query;
		}

		//Verificar se já existe um registro com a descrição informada, no entanto, que possua id diferente do informado
		public bool existe(AreaAtuacao OAreaAtuacao, int id) {

			var query = from C in db.AreaAtuacao
						where 
                            C.descricao == OAreaAtuacao.descricao && 
                            C.id != id && 
                            C.flagExcluido == "N"
						select C;

            query = query.condicoesSeguranca();

			return query.Any();
		}

		//Realizar os tratamentos necessários
		//Salvar um novo registro
		public bool salvar(AreaAtuacao OAreaAtuacao) {

			if(OAreaAtuacao.id == 0){	
				return this.inserir(OAreaAtuacao);
			}

			return this.atualizar(OAreaAtuacao);
		}

        //Persistir e inserir um novo registro 
		//Inserir AreaAtuacao
        private bool inserir(AreaAtuacao OAreaAtuacao) { 

			OAreaAtuacao.setDefaultInsertValues<AreaAtuacao>();

			db.AreaAtuacao.Add(OAreaAtuacao);

			db.SaveChanges();

			return OAreaAtuacao.id > 0;
		}

        //Persistir e atualizar um registro existente 
		//Atualizar dados da AreaAtuacao
		private bool atualizar(AreaAtuacao OAreaAtuacao) { 

			//Localizar existentes no banco
			AreaAtuacao dbAreaAtuacao = this.carregar(OAreaAtuacao.id);

            if(dbAreaAtuacao == null) {
                return false;
            }

			//Configurar valores padrão
			OAreaAtuacao.setDefaultUpdateValues();

			//Atualizacao da AreaAtuacao
			var AreaAtuacaoEntry = db.Entry(dbAreaAtuacao);
			AreaAtuacaoEntry.CurrentValues.SetValues(OAreaAtuacao);
			AreaAtuacaoEntry.ignoreFields();

			db.SaveChanges();

			return OAreaAtuacao.id > 0;
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

		// Excluir Registro
		public UtilRetorno excluir(int id, int idUsuarioExclusao) {

		    var ORegistro = this.carregar(id);

		    if (ORegistro == null) {
		        return UtilRetorno.newInstance(true, "O registro informado não pôde ser localizado.");
		    }

		    ORegistro.flagExcluido = "S";

            ORegistro.idUsuarioAlteracao = idUsuarioExclusao;

            ORegistro.dtAlteracao = DateTime.Now;

            db.SaveChanges();

		    return UtilRetorno.newInstance(false, "Os dados foram atualizados com sucesso.");
		}
	}
}