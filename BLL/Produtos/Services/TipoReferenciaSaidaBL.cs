using System;
using System.Linq;
using DAL.Produtos;
using EntityFramework.Extensions;
using System.Json;
using BLL.Services;
using DAL.Permissao.Security.Extensions;
using UTIL.Resources;

namespace BLL.Produtos {

	public class TipoReferenciaSaidaBL : DefaultBL, ITipoReferenciaSaidaBL {

		//
		public TipoReferenciaSaidaBL() {
		}

		//Carregamento de registro pelo ID
		public TipoReferenciaSaida carregar(int id) { 

			return db.TipoReferenciaSaida.Find(id);
		}

		//Listagem de registros de acordo com filtros
		public IQueryable<TipoReferenciaSaida> listar(string valorBusca, string ativo) {

			var query = from P in db.TipoReferenciaSaida
						where P.flagExcluido == "N"
						select P;

			if (!String.IsNullOrEmpty(valorBusca)) {
				query = query.Where(x => x.descricao.Contains(valorBusca));
			}

			if (!String.IsNullOrEmpty(ativo)) {
				query = query.Where(x => x.ativo == ativo);
			}

			return query;
		}

		//Verificar se já existe um registro com a descrição informada, no entanto, que possua id diferente do informado
		public bool existe(string descricao, int id) {

			var query = from P in db.TipoReferenciaSaida
						where P.descricao == descricao && P.id != id && P.flagExcluido == "N"
						select P;
			var OTipoReferenciaSaida = query.Take(1).FirstOrDefault();
			return (OTipoReferenciaSaida != null);
		}

		//Verificar se deve-se atualizar um registro existente ou criar um novo
		public bool salvar(TipoReferenciaSaida OTipoReferenciaSaida) {

			if (OTipoReferenciaSaida.id == 0) { 
				return this.inserir(OTipoReferenciaSaida);
			}

			return this.atualizar(OTipoReferenciaSaida);
		}

		//Persistir o objecto e salvar na base de dados
		private bool inserir(TipoReferenciaSaida OTipoReferenciaSaida) { 

			OTipoReferenciaSaida.setDefaultInsertValues<TipoReferenciaSaida>();
			db.TipoReferenciaSaida.Add(OTipoReferenciaSaida);
			db.SaveChanges();

			return (OTipoReferenciaSaida.id > 0);
		}

		//Persistir o objecto e atualizar informações
		private bool atualizar(TipoReferenciaSaida OTipoReferenciaSaida) { 

			OTipoReferenciaSaida.setDefaultUpdateValues<TipoReferenciaSaida>();

			//Localizar existentes no banco
			TipoReferenciaSaida dbTipoReferenciaSaida = this.carregar(OTipoReferenciaSaida.id);		
			var TipoEntry = db.Entry(dbTipoReferenciaSaida);
			TipoEntry.CurrentValues.SetValues(OTipoReferenciaSaida);
            TipoEntry.ignoreFields<TipoReferenciaSaida>();

			db.SaveChanges();
			return (OTipoReferenciaSaida.id > 0);
		}

        public JsonMessageStatus alterarStatus(int id) {
            var retorno = new JsonMessageStatus();

            TipoReferenciaSaida Objeto = this.carregar(id);
            if (Objeto == null) {
                retorno.error = true;
                retorno.message = NotificationMessages.invalid_register_id;
            } else {
                Objeto.ativo = (Objeto.ativo == "S" ? "N" : "S");
                db.SaveChanges();
                retorno.active = Objeto.ativo;
                retorno.message = "Os dados foram alterados com sucesso.";
            }
            return retorno;
        }

		//Remover um registro (exclusao lógica - nao remove-se fisicamente)
		public bool excluir(int id) {

		    var idUsuario = User.id();

			db.TipoReferenciaSaida
				.Where(x => x.id == id)
				.Update(x => new TipoReferenciaSaida{flagExcluido = "S", dtAlteracao = DateTime.Now, idUsuarioAlteracao = idUsuario });

			return true;
		}
	}
}