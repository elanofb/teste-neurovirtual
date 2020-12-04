using System;
using System.Json;
using System.Linq;
using BLL.Services;
using DAL.Cargos;
using UTIL.Resources;

namespace BLL.Cargos {

	public class CargoBL : DefaultBL, ICargoBL {

        //Atributos

        //Propriedades

		//
		public CargoBL(){
		}

        //Carregamento de registro único pelo ID
		public Cargo carregar(int id) {
			
			var query = from Item in db.Cargo
						where 
							Item.id == id && 
							Item.flagExcluido == "N"
						select Item;

            query = query.condicoesSeguranca();

			return query.FirstOrDefault();
		}

		//
		public IQueryable<Cargo> listar(string valorBusca, string ativo) {
			
			var query = from C in db.Cargo
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
		public bool existe(Cargo OCargo, int id) {

			var query = from C in db.Cargo
						where 
                            C.descricao == OCargo.descricao && 
                            C.id != id && 
                            C.flagExcluido == "N"
						select C;

            query = query.condicoesSeguranca();

			return query.Any();
		}

		//Realizar os tratamentos necessários
		//Salvar um novo registro
		public bool salvar(Cargo OCargo) {

			if(OCargo.id == 0){	
				return this.inserir(OCargo);
			}

			return this.atualizar(OCargo);
		}

        //Persistir e inserir um novo registro 
		//Inserir Cargo
        private bool inserir(Cargo OCargo) { 

			OCargo.setDefaultInsertValues<Cargo>();

			db.Cargo.Add(OCargo);

			db.SaveChanges();

			return OCargo.id > 0;
		}

        //Persistir e atualizar um registro existente 
		//Atualizar dados da Cargo
		private bool atualizar(Cargo OCargo) { 

			//Localizar existentes no banco
			Cargo dbCargo = this.carregar(OCargo.id);

            if (dbCargo == null) {
                return false;
            }

			//Configurar valores padrão
			OCargo.setDefaultUpdateValues();

			//Atualizacao da Cargo
			var CargoEntry = db.Entry(dbCargo);
			CargoEntry.CurrentValues.SetValues(OCargo);
			CargoEntry.ignoreFields();

			db.SaveChanges();

			return OCargo.id > 0;
		}

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