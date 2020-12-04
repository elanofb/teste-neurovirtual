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

	public class DetalheTipoCategoriaTituloBL : DefaultBL , IDetalheTipoCategoriaTituloBL {

		//
		public DetalheTipoCategoriaTituloBL() {
		}

        //Carregamento de registro pelo ID
        public DetalheTipoCategoriaTitulo carregar(int id) {

            var query = from P in db.DetalheTipoCategoriaTitulo.Include(x => x.TipoCategoria)
						where P.id == id && P.flagExcluido == "N"
						select P;

            query = query.condicoesSeguranca();

            return query.FirstOrDefault();
        }

        //Listagem de registros de acordo com filtros
		public IQueryable<DetalheTipoCategoriaTitulo> listar(int idMacroConta, int idCategoria, int idTipoCategoria, string valorBusca, string ativo) {
			
            var query = from P in db.DetalheTipoCategoriaTitulo.Include(x => x.TipoCategoria)
						where P.flagExcluido == "N"
						select P;

            query = query.condicoesSeguranca();

			if (idMacroConta > 0) {
				query = query.Where(x => x.TipoCategoria.Categoria.idMacroConta == idMacroConta);
			}

			if (idCategoria > 0) {
				query = query.Where(x => x.TipoCategoria.idCategoria == idCategoria);
			}

			if (idTipoCategoria > 0) {
				query = query.Where(x => x.idTipoCategoria == idTipoCategoria);
			}

			if (!String.IsNullOrEmpty(valorBusca)) {
				query = query.Where(x => x.descricao.Contains(valorBusca));
			}

			if (!String.IsNullOrEmpty(ativo)) {
				query = query.Where(x => x.ativo == ativo);
			}

			return query;
		}

        //Verificar se já existe um registro com a descrição informada, no entanto, que possua id diferente do informado
		public bool existe(int? idTipoCategoria, string descricao, int id) {
            
			var query = from P in db.DetalheTipoCategoriaTitulo
						where P.descricao == descricao && P.id != id && P.flagExcluido == "N"
                        && P.idTipoCategoria == idTipoCategoria
						select P;

            query = query.condicoesSeguranca();

			var ODetalheTipoCategoriaTitulo = query.Take(1).FirstOrDefault();

			return (ODetalheTipoCategoriaTitulo == null ? false : true);
		}

        //Verificar se deve-se atualizar um registro existente ou criar um novo
        public bool salvar(DetalheTipoCategoriaTitulo OTipoProduto) {

            OTipoProduto.TipoCategoria = null;

            if(OTipoProduto.id == 0) {
                return this.inserir(OTipoProduto);
            }

            return this.atualizar(OTipoProduto);
        }

        //Persistir o objecto e salvar na base de dados
        private bool inserir(DetalheTipoCategoriaTitulo ODetalheTipoCategoriaTitulo) {

            ODetalheTipoCategoriaTitulo.setDefaultInsertValues<DetalheTipoCategoriaTitulo>();
            db.DetalheTipoCategoriaTitulo.Add(ODetalheTipoCategoriaTitulo);
            db.SaveChanges();

            return (ODetalheTipoCategoriaTitulo.id > 0);
        }

        //Persistir o objecto e atualizar informações
        private bool atualizar(DetalheTipoCategoriaTitulo ODetalheTipoCategoriaTitulo) {

            ODetalheTipoCategoriaTitulo.setDefaultUpdateValues<DetalheTipoCategoriaTitulo>();

            //Localizar existentes no banco
            DetalheTipoCategoriaTitulo dbDetalheTipoCategoriaTitulo = this.carregar(ODetalheTipoCategoriaTitulo.id);

            if (dbDetalheTipoCategoriaTitulo == null) {
                return false;
            }

            var TipoEntry = db.Entry(dbDetalheTipoCategoriaTitulo);
            TipoEntry.CurrentValues.SetValues(ODetalheTipoCategoriaTitulo);
            TipoEntry.ignoreFields<DetalheTipoCategoriaTitulo>();

            db.SaveChanges();
            return (ODetalheTipoCategoriaTitulo.id > 0);
        }

        //Remover um registro (exclusao lógica - nao remove-se fisicamente)
        public bool excluir(int id) {

            var idUsuario = User.id();

            db.DetalheTipoCategoriaTitulo
                .Where(x => x.id == id)
                .Update(x => new DetalheTipoCategoriaTitulo { flagExcluido = "S",dtAlteracao = DateTime.Now,idUsuarioAlteracao = idUsuario });

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

		public IQueryable<DetalheTipoCategoriaTitulo> autocompletar(int idTipoCategoria) {

			var query = (from C in db.DetalheTipoCategoriaTitulo
						 where 
							C.idTipoCategoria == idTipoCategoria &&
							C.flagExcluido == "N" 
						 select C);			
			
            query = query.condicoesSeguranca();

			return query;
		}
    }
}