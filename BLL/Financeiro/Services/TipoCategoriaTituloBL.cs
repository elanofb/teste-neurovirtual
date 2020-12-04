using System;
using System.Linq;
using DAL.Financeiro;
using EntityFramework.Extensions;
using System.Json;
using UTIL.Resources;
using System.Data.Entity;
using BLL.Services;
using DAL.Permissao.Security.Extensions;

namespace BLL.Financeiro {

	public class TipoCategoriaTituloBL : DefaultBL, ITipoCategoriaTituloBL {

		//
		public TipoCategoriaTituloBL() {
		}

        //Carregamento de registro pelo ID
        public TipoCategoriaTitulo carregar(int id) {

            var query = from P in db.TipoCategoriaTitulo
                                    .Include(x => x.Categoria)
                                    .Include(x => x.Categoria.MacroConta)
						where P.id == id && P.flagExcluido == "N"
						select P;

            query = query.condicoesSeguranca();

            return query.FirstOrDefault();
        }

		public IQueryable<TipoCategoriaTitulo> listar(int idMacroConta, int idCategoria, string valorBusca, string ativo) {

			var query = from P in db.TipoCategoriaTitulo
                                    .Include(x => x.Categoria)
                                    .Include(x => x.Categoria.MacroConta)
						where P.flagExcluido == "N"
						select P;

            query = query.condicoesSeguranca();

			if (idMacroConta > 0) {
				query = query.Where(x => x.Categoria.idMacroConta == idMacroConta);
			}

			if (idCategoria > 0) {
				query = query.Where(x => x.idCategoria == idCategoria);
			}

			if (!String.IsNullOrEmpty(valorBusca)) {
				query = query.Where(x => x.descricao.Contains(valorBusca));
			}

			if (!String.IsNullOrEmpty(ativo)) {
				query = query.Where(x => x.ativo == ativo);
			}

			return query;
		}

		public bool existe(int? idCategoria, string descricao, int id) {

			var query = from P in db.TipoCategoriaTitulo
						where P.descricao == descricao && P.id != id && P.flagExcluido == "N"
                        && P.idCategoria == idCategoria
						select P;

            query = query.condicoesSeguranca();

			var OTipoCategoriaTitulo = query.Take(1).FirstOrDefault();
			return (OTipoCategoriaTitulo != null);
		}

        public bool salvar(TipoCategoriaTitulo OTipoProduto) {
            
            OTipoProduto.Categoria = null;

            if(OTipoProduto.id == 0) {
                return this.inserir(OTipoProduto);
            }

            return this.atualizar(OTipoProduto);
        }

        private bool inserir(TipoCategoriaTitulo OTipoCategoriaTitulo) {
            
            OTipoCategoriaTitulo.setDefaultInsertValues<TipoCategoriaTitulo>();
            db.TipoCategoriaTitulo.Add(OTipoCategoriaTitulo);
            db.SaveChanges();

            return (OTipoCategoriaTitulo.id > 0);
        }

        private bool atualizar(TipoCategoriaTitulo OTipoCategoriaTitulo) {

            OTipoCategoriaTitulo.setDefaultUpdateValues<TipoCategoriaTitulo>();

            //Localizar existentes no banco
            TipoCategoriaTitulo dbTipoCategoriaTitulo = this.carregar(OTipoCategoriaTitulo.id);

            if (dbTipoCategoriaTitulo == null) {
                return false;
            }

            var TipoEntry = db.Entry(dbTipoCategoriaTitulo);
            TipoEntry.CurrentValues.SetValues(OTipoCategoriaTitulo);
            TipoEntry.ignoreFields<TipoCategoriaTitulo>();

            db.SaveChanges();
            return (OTipoCategoriaTitulo.id > 0);
        }

        public bool excluir(int id) {

            var idUsuario = User.id();

            db.TipoCategoriaTitulo
                .Where(x => x.id == id)
                .Update(x => new TipoCategoriaTitulo { flagExcluido = "S",dtAlteracao = DateTime.Now,idUsuarioAlteracao = idUsuario });

            return true;
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
            return retorno;
        }

		public IQueryable<TipoCategoriaTitulo> autocompletar(int idCategoria) {

			var query = (from C in db.TipoCategoriaTitulo
						 where 
							C.idCategoria == idCategoria &&
							C.flagExcluido == "N" 
						 select C);			
			
            query = query.condicoesSeguranca();

			return query;
		}

    }
}