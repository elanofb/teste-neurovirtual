using System;
using System.Json;
using System.Linq;
using DAL.Unidades;
using DAL.Repository.Base;
using EntityFramework.Extensions;
using UTIL.Resources;
using System.Data.Entity;
using BLL.Services;

namespace BLL.Unidades {

	public class UnidadeContaBancariaBL : DefaultBL {
		/**
		*
		*/

		public UnidadeContaBancariaBL(){

		}

		/**
		*
		*/

		public UnidadeContaBancaria carregar(int id) {
            
			var OUnidadeContaBancaria = db.UnidadeContaBancaria.SingleOrDefault(x => x.id == id && x.flagExcluido == "N");
			return OUnidadeContaBancaria;
		}

		/**
		*
		*/

		public IQueryable<UnidadeContaBancaria> listar(int idUnidade, int idBanco, string ativo) {
            
			var query = from Obj in db.UnidadeContaBancaria
                                    .Include(x => x.Unidade)
                                    .Include(x => x.Banco)

						where Obj.flagExcluido == "N"
						select Obj;

			if (idUnidade > 0) {
                query = query.Where(x => x.idUnidade == idUnidade);
			}

            if (idBanco > 0) {
				query = query.Where(x => x.idBanco == idBanco);
			}

			if (!String.IsNullOrEmpty(ativo)) {
				query = query.Where(x => x.ativo == ativo);
			}

			return query;
		}

		//
		public bool salvar(UnidadeContaBancaria OUnidadeContaBancaria) {

            OUnidadeContaBancaria.nroAgencia = UtilString.onlyNumber(OUnidadeContaBancaria.nroAgencia);
            OUnidadeContaBancaria.nroConta = UtilString.onlyNumber(OUnidadeContaBancaria.nroConta);

            if (OUnidadeContaBancaria.id == 0) {
                return this.inserir(OUnidadeContaBancaria);
            }

            return this.atualizar(OUnidadeContaBancaria);
		}

        private bool inserir(UnidadeContaBancaria OUnidadeContaBancaria) {
            
            OUnidadeContaBancaria.setDefaultInsertValues<UnidadeContaBancaria>();
            db.UnidadeContaBancaria.Add(OUnidadeContaBancaria);
            db.SaveChanges();
            return (OUnidadeContaBancaria.id > 0);
        }

        private bool atualizar(UnidadeContaBancaria OUnidadeContaBancaria) {
            

            UnidadeContaBancaria dbUnidadeContaBancaria = this.carregar(OUnidadeContaBancaria.id);

            var TipoEntry = db.Entry(dbUnidadeContaBancaria);
			OUnidadeContaBancaria.setDefaultUpdateValues<UnidadeContaBancaria>();
            TipoEntry.CurrentValues.SetValues(OUnidadeContaBancaria);
            TipoEntry.State = System.Data.Entity.EntityState.Modified;
            TipoEntry.ignoreFields<UnidadeContaBancaria>();
            
            db.SaveChanges();
            return (OUnidadeContaBancaria.id > 0);
        }

        //
		public JsonMessageStatus alterarStatus(int id) {
			var retorno = new JsonMessageStatus();

			UnidadeContaBancaria item = this.carregar(id);
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
		public JsonMessage delete(int[] id) {
            
			var itens = db.UnidadeContaBancaria.Where(x => id.Contains(x.id)).Update(x => new UnidadeContaBancaria { flagExcluido = "S", dtAlteracao = DateTime.Now });
			return new JsonMessage { error = false, message = NotificationMessages.delete_success };
		}

		//
		public bool excluir(int[] ids) {
            
			db.UnidadeContaBancaria.Where(x => ids.Contains(x.id))
				.Update(x => new UnidadeContaBancaria { flagExcluido = "S", dtAlteracao = DateTime.Now });

			var listaCheck = db.UnidadeContaBancaria.Where(x => ids.Contains(x.id) && x.flagExcluido == "N").ToList();
			return (listaCheck.Count == 0);
		}

		public object getAutoComplete(string term) {
			
			var query = from p in db.UnidadeContaBancaria.Include(x => x.Banco)
						where
							((p.Banco.descricao.Contains(term) && !p.flagExcluido.Equals("S") && p.ativo.Equals("S")))
						select new {
							value = p.Banco.descricao,
							nome = p.Banco.descricao,
							id = p.id,
						};

			return query.ToList();
		}

        public bool existe(string nroAgencia, string nroConta, int id) {
			
			var query = from Obj in db.UnidadeContaBancaria
						where 
                            Obj.nroAgencia == nroAgencia && Obj.nroConta == nroConta && Obj.id != id && Obj.flagExcluido == "N"
						select Obj;
			var OCliente = query.Take(1).FirstOrDefault();
			return (OCliente == null ? false : true);
		}

	}
}