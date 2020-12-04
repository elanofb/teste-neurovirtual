using BLL.Services;
using DAL.Localizacao;
using System;
using System.Json;
using System.Linq;
using UTIL.Resources;

namespace BLL.Localizacao {

    public class PaisBL : DefaultBL, IPaisBL {

		//
		public PaisBL() {

		}
        
		//Busca de registro único a partir do ID
		public Pais carregar(string id) {
			
			var query = (from OPais in db.Pais where OPais.id == id && OPais.flagExcluido == "N" select OPais);

			return query.FirstOrDefault();
		}
        
		//Listagem de registros com base nos parâmetros de busca
		public IQueryable<Pais> listar(string valorBusca, string ativo) {
			
			var query = from OPais in db.Pais
						where OPais.flagExcluido == "N"
						select OPais;

			if (!String.IsNullOrEmpty(valorBusca)) {
				query = query.Where(x => x.nome.Contains(valorBusca));
			}

			if (!String.IsNullOrEmpty(ativo)) {
				query = query.Where(x => x.ativo == ativo);
			}

			return query;
		}
        
        //Persistir o objecto e salvar na base de dados
        public bool inserir(Pais OPais) {
            
            OPais.setDefaultInsertValues();
            db.Pais.Add(OPais);
            db.SaveChanges();

            return !String.IsNullOrEmpty(OPais.id);
        }

        //Persistir o objecto e atualizar informações
        public bool atualizar(Pais OPais) {
            
            OPais.setDefaultUpdateValues();

            //Localizar existentes no banco
            Pais dbPais = this.carregar(OPais.id);
            var TipoEntry = db.Entry(dbPais);
            TipoEntry.CurrentValues.SetValues(OPais);
            TipoEntry.ignoreFields();

            db.SaveChanges();
            return !String.IsNullOrEmpty(OPais.id);
        }

        //Ativacao - Desativacao de registro
		public JsonMessageStatus alterarStatus(string id) {
			var retorno = new JsonMessageStatus();

			Pais item = this.carregar(id);
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

		//
		public UtilRetorno excluir(string id, int idUsuarioExclusao) {

			var OPais = this.carregar(id);

		    if (OPais == null) {
		        return UtilRetorno.newInstance(true, "A ocorrência informada não foi localizado.");
		    }

            OPais.flagExcluido = "S";

		    OPais.idUsuarioAlteracao = idUsuarioExclusao;

		    db.SaveChanges();

            return UtilRetorno.newInstance(false, "Registro removido com sucesso.");

		}
	}
}