using System;
using System.Linq;
using DAL.Financeiro;
using DAL.Repository.Base;
using EntityFramework.Extensions;
using System.Json;
using BLL.Caches;
using BLL.Services;
using DAL.Permissao.Security.Extensions;
using UTIL.Resources;

namespace BLL.Financeiro {

	public class CategoriaTituloBL : DefaultBL , ICategoriaTituloBL {

	    public const string keyCache = "sub_conta";

		//
		public CategoriaTituloBL(){
		}

        //Carregamento de registro pelo ID
        public CategoriaTitulo carregar(int id) {

            var query = from P in db.CategoriaTitulo
						where P.id == id && P.flagExcluido == "N"
						select P;

            query = query.condicoesSeguranca();

            return query.FirstOrDefault();

        }

        //Listagem de registros de acordo com filtros
		public IQueryable<CategoriaTitulo> listar(int idMacroConta, string valorBusca, string ativo) {

			var query = from P in db.CategoriaTitulo
						where P.flagExcluido == "N"
						select P;

            query = query.condicoesSeguranca();

			if (idMacroConta > 0) {
				query = query.Where(x => x.idMacroConta == idMacroConta);
			}

			if (!String.IsNullOrEmpty(valorBusca)) {
				query = query.Where(x => x.descricao.Contains(valorBusca));
			}

			if (!String.IsNullOrEmpty(ativo)) {
				query = query.Where(x => x.ativo == ativo);
			}

			return query;
		}

        //Verificar se deve-se atualizar um registro existente ou criar um novo
        public bool salvar(CategoriaTitulo OTipoProduto) {

            OTipoProduto.MacroConta = null;

	        var flagSucesso = false;
	        
            if(OTipoProduto.id == 0) {
	            flagSucesso = this.inserir(OTipoProduto);
            }

	        flagSucesso = this.atualizar(OTipoProduto);

	        if (flagSucesso) {
		        CacheService.getInstance.remover(keyCache);
	        }

	        return flagSucesso;
        }

        //Persistir o objecto e salvar na base de dados
        private bool inserir(CategoriaTitulo OCategoriaTitulo) {

            OCategoriaTitulo.setDefaultInsertValues<CategoriaTitulo>();
            db.CategoriaTitulo.Add(OCategoriaTitulo);
            db.SaveChanges();

            return (OCategoriaTitulo.id > 0);
        }

        //Persistir o objecto e atualizar informações
        private bool atualizar(CategoriaTitulo OCategoriaTitulo) {

            OCategoriaTitulo.setDefaultUpdateValues<CategoriaTitulo>();

            //Localizar existentes no banco
            CategoriaTitulo dbCategoriaTitulo = this.carregar(OCategoriaTitulo.id);

            if (dbCategoriaTitulo == null) {
                return false;
            }

            var TipoEntry = db.Entry(dbCategoriaTitulo);
            TipoEntry.CurrentValues.SetValues(OCategoriaTitulo);
            TipoEntry.ignoreFields<CategoriaTitulo>();

            db.SaveChanges();
            return (OCategoriaTitulo.id > 0);
        }

        //Remover um registro (exclusao lógica - nao remove-se fisicamente)
        public bool excluir(int id) {

            var idUsuario = User.id();

            db.CategoriaTitulo
                .Where(x => x.id == id)
                .Update(x => new CategoriaTitulo { flagExcluido = "S",dtAlteracao = DateTime.Now,idUsuarioAlteracao = idUsuario });

	        CacheService.getInstance.remover(keyCache);
	        
            return true;
        }

		public bool existe(int? idMacroConta, string descricao, int id) {

			var query = from P in db.CategoriaTitulo
						where P.descricao == descricao && P.id != id && P.flagExcluido == "N"
                              && P.idMacroConta == idMacroConta
						select P;

            query = query.condicoesSeguranca();

			var OCategoriaTitulo = query.Take(1).FirstOrDefault();
			return (OCategoriaTitulo != null);
		}

        public JsonMessageStatus alterarStatus(int id) {

            var retorno = new JsonMessageStatus();

            var Objeto = this.carregar(id);
	        
            if (Objeto == null) {
                retorno.error = true;
                retorno.message = NotificationMessages.invalid_register_id;
            } else {
                Objeto.ativo = (Objeto.ativo == "S" ? "N" : "S");
                db.SaveChanges();
                retorno.active = Objeto.ativo;
                retorno.message = "Os dados foram alterados com sucesso.";
            }

	        if (!retorno.error) {
		        CacheService.getInstance.remover(keyCache);
	        }

	        return retorno;
        }

		public IQueryable<CategoriaTitulo> autocompletar(int idMacroConta) {

			var query = (from C in db.CategoriaTitulo
						 where 
							C.idMacroConta == idMacroConta &&
							C.flagExcluido == "N" 
						 select C);			
			
            query = query.condicoesSeguranca();

			return query;
		}
    }
}