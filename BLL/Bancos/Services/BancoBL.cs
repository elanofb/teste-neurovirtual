using BLL.Services;
using DAL.Bancos;
using System;
using System.Json;
using System.Linq;
using UTIL.Resources;

namespace BLL.Bancos {

    public class BancoBL : DefaultBL, IBancoBL {

        //
        public BancoBL() {

        }

        //Carregamento de registro pelo ID
        public Banco carregar(int id) {
            
            return db.Banco.Find(id);
        }

        //Listagem de registros de acordo com filtros
        public IQueryable<Banco> listar(string valorBusca,string ativo) {
            
            var query = from P in db.Banco
                        where P.flagExcluido == "N"
                        select P;

            if(!String.IsNullOrEmpty(valorBusca)) {
                query = query.Where(x => x.descricao.Contains(valorBusca) || x.nome.Contains(valorBusca));
            }

            if(!String.IsNullOrEmpty(ativo)) {
                query = query.Where(x => x.ativo == ativo);
            }

            return query;
        }

        //Verificar se já existe um registro com a descrição informada, no entanto, que possua id diferente do informado
        public bool existe(string descricao, string nro, int id) {

            
            var query = from P in db.Banco
                        where P.descricao == descricao && P.nroBanco == nro && P.id != id && P.flagExcluido == "N"
                        select P;
            var OBanco = query.Take(1).FirstOrDefault();

            return (OBanco != null);
        }

        //Verificar se deve-se atualizar um registro existente ou criar um novo
        public bool salvar(Banco OBanco) {

            if(OBanco.id == 0) {
                return this.inserir(OBanco);
            }

            return this.atualizar(OBanco);
        }

        //Persistir o objecto e salvar na base de dados
        private bool inserir(Banco OBanco) {
            
            OBanco.setDefaultInsertValues();
            db.Banco.Add(OBanco);
            db.SaveChanges();

            return (OBanco.id > 0);
        }

        //Persistir o objecto e atualizar informações
        private bool atualizar(Banco OBanco) {
            
            OBanco.setDefaultUpdateValues();

            //Localizar existentes no banco
            Banco dbBanco = this.carregar(OBanco.id);
            var TipoEntry = db.Entry(dbBanco);
            TipoEntry.CurrentValues.SetValues(OBanco);
            TipoEntry.ignoreFields();

            db.SaveChanges();
            return (OBanco.id > 0);
        }

        //Ativacao - Desativacao de registro
		public JsonMessageStatus alterarStatus(int id) {
			var retorno = new JsonMessageStatus();

			Banco item = this.carregar(id);
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
        public UtilRetorno excluir(int id, int idUsuarioExclusao) {
            
            var OBanco = this.carregar(id);

		    if (OBanco == null) {
		        return UtilRetorno.newInstance(true, "O banco informado não foi localizado.");
		    }

            OBanco.flagExcluido = "S";

		    OBanco.idUsuarioAlteracao = idUsuarioExclusao;

		    db.SaveChanges();

            return UtilRetorno.newInstance(false, "Registro removido com sucesso.");
            
        }
        
    }
}