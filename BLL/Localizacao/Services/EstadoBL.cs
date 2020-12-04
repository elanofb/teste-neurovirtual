using BLL.Services;
using DAL.Localizacao;
using System;
using System.Json;
using System.Linq;
using UTIL.Resources;

namespace BLL.Localizacao {

    public class EstadoBL : DefaultBL, IEstadoBL {

		//
		public EstadoBL() {

		}

		//Carregar cidade pela sigla informado
		public Estado carregar(string sigla) {
			
			var query = from E in db.Estado 
                        where E.flagExcluido == "N" &&
                        E.sigla == sigla
                        select E;

            return query.FirstOrDefault();
		}

        //Carregar cidade pelo id informado
		public Estado carregarPorId(int id) {
			
			var query = from E in db.Estado 
                        where E.flagExcluido == "N" &&
                        E.id == id
                        select E;

            return query.FirstOrDefault();
		}

		//
		public IQueryable<Estado> listar(string valorBusca, string ativo) {
			
			var query = from Est in db.Estado
						where Est.flagExcluido == "N"
						select Est;

			if (!String.IsNullOrEmpty(valorBusca)) {
				query = query.Where(x => x.nome.Contains(valorBusca));
			}

			if (!String.IsNullOrEmpty(ativo)) {
				query = query.Where(x => x.ativo == ativo);
			}

			return query;
		}

        //Verificar se deve-se atualizar um registro existente ou criar um novo
        public bool salvar(Estado OEstado) {

            if(OEstado.id == 0) {
                return this.inserir(OEstado);
            }

            return this.atualizar(OEstado);
        }

        //Persistir o objecto e salvar na base de dados
        private bool inserir(Estado OEstado) {
            
            OEstado.setDefaultInsertValues();
            db.Estado.Add(OEstado);
            db.SaveChanges();

            return (OEstado.id > 0);
        }

        //Persistir o objecto e atualizar informações
        private bool atualizar(Estado OEstado) {
            
            OEstado.setDefaultUpdateValues();

            //Localizar existentes no banco
            Estado dbEstado = this.carregarPorId(OEstado.id);
            var TipoEntry = db.Entry(dbEstado);
            TipoEntry.CurrentValues.SetValues(OEstado);
            TipoEntry.ignoreFields();

            db.SaveChanges();
            return (OEstado.id > 0);
        }

        //Ativacao - Desativacao de registro
		public JsonMessageStatus alterarStatus(int id) {
			var retorno = new JsonMessageStatus();

			Estado item = this.carregarPorId(id);
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

        //Remover um registro (exclusao lógica - nao remove-se fisicamente)
        public UtilRetorno excluir(int id) {
            
            var OEstado = this.carregarPorId(id);

		    if (OEstado == null) {
		        return UtilRetorno.newInstance(true, "O estado informado não foi localizado.");
		    }

            OEstado.flagExcluido = "S";
            
		    db.SaveChanges();

            return UtilRetorno.newInstance(false, "Registro removido com sucesso.");
            
        }

	}
}