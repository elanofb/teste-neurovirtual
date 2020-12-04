using System;
using System.Data.Entity;
using System.Linq;
using BLL.Services;
using DAL.Associados;
using DAL.Permissao.Security.Extensions;
using EntityFramework.Extensions;

namespace BLL.Associados {
    //preenche um obj do tipo "Tipo associado" com os dados do banco
    public class TipoAssociadoBL : DefaultBL, ITipoAssociadoBL {

		/*Rotinas de consulta*/
		//criador de uma query genérica acessivel para as rotinas de consulta
		public IQueryable<TipoAssociado> query(int? idOrganizacaoParam = null) {

			var query = from TA in db.TipoAssociado
				where TA.flagExcluido == "N"
				select TA;

			if (idOrganizacaoParam == null) {
				idOrganizacaoParam = idOrganizacao;
			}

			if (idOrganizacaoParam > 0) {
				query = query.Where(x => x.idOrganizacao == idOrganizacaoParam);
			}

			return query;

		}
		//preenche um obj do tipo "Tipo associado" com os dados do banco
        public TipoAssociado carregar(int id, int? idOrganizacaoInf = null) {

            if (idOrganizacaoInf.toInt() == 0) {
                idOrganizacaoInf = idOrganizacao;
            }

            var query = (from T in db.TipoAssociado
						 where
							T.flagExcluido == "N" &&
							T.id == id
                         select T);


            if (idOrganizacaoInf > 0) {
                query = query.Where(x => x.idOrganizacao == idOrganizacaoInf);
            }

            if (idOrganizacaoInf == 0) {
                query = query.Where(x => x.idOrganizacao == null);
            }

            return query.FirstOrDefault();
		}
		//preenche um obj do tipo "Tipo associado" com os dados do banco com a descricao
        public TipoAssociado carregarPorDescricao(string descricao, int idCategoria, int? idOrganizacaoInf = null) {

            if (idOrganizacao > 0 && idOrganizacaoInf == null) {
                idOrganizacaoInf = idOrganizacao;
            }

            var query = (from T in db.TipoAssociado
						 where
							T.flagExcluido == "N" &&
							(T.descricao == descricao || T.nomeDisplay == descricao)
                         select T);


            if (idOrganizacaoInf > 0) {
                query = query.Where(x => x.idOrganizacao == idOrganizacaoInf);
            }

            if (idCategoria > 0) {
                query = query.Where(x => x.idCategoria == idCategoria);
            }

            if (idOrganizacaoInf == 0) {
                query = query.Where(x => x.idOrganizacao == null);
            }

            return query.FirstOrDefault();
		}
		//cria uma lista com todos os conteudos do banco em objs do tipo TipoAssociado
        public IQueryable<TipoAssociado> listar(string valorBusca, bool? flagIsento, string ativo, int? idOrganizacaoParam = null) {

            var query = from T in db.TipoAssociado.Include(x => x.Categoria)
						where T.flagExcluido == "N"
						select T;

            if (flagIsento.HasValue) {
				query = query.Where(x => x.flagIsento == flagIsento);
			}

			if (!String.IsNullOrEmpty(valorBusca)) {
				query = query.Where(x => x.descricao.Contains(valorBusca));
			}

			if (!String.IsNullOrEmpty(ativo)) {
				query = query.Where(x => x.ativo == ativo);
			}

            if (idOrganizacaoParam == null) {
                idOrganizacaoParam = idOrganizacao;
            }

            if (idOrganizacaoParam > 0) {
                query = query.Where(x => x.idOrganizacao == idOrganizacaoParam);
            }

			return query;
		}

        /*Rotinas de Cadastro*/
        //Verificar se deve-se atualizar um registro existente ou criar um novo
        public bool salvar(TipoAssociado OTipoAssociado) {
            if (OTipoAssociado.id == 0) {
                return this.inserir(OTipoAssociado);
            }

            return this.atualizar(OTipoAssociado);
        }
        //Persistir o objecto e salvar na base de dados
        private bool inserir(TipoAssociado OTipoAssociado) {
            OTipoAssociado.setDefaultInsertValues<TipoAssociado>();

            db.TipoAssociado.Add(OTipoAssociado);

            db.SaveChanges();

            return (OTipoAssociado.id > 0);
        }
        //Persistir o objecto e atualizar informações
        private bool atualizar(TipoAssociado OTipoAssociado) {
            //Localizar existentes no banco
            TipoAssociado dbTipoAssociado = this.carregar(OTipoAssociado.id);

            if (dbTipoAssociado == null) {
                return false;
            }

            OTipoAssociado.setDefaultUpdateValues<TipoAssociado>();

            var TipoEntry = db.Entry(dbTipoAssociado);
            TipoEntry.CurrentValues.SetValues(OTipoAssociado);
            TipoEntry.ignoreFields(new[] {"flagSistema"});

            db.SaveChanges();
            return (OTipoAssociado.id > 0);
        }
        //Verificar se já existe um registro para evitar duplicidades
        private int proximoId() {
            int nroProximoId = db.TipoAssociado.DefaultIfEmpty().Max(x => x.id);

            if (nroProximoId < 100) {
                nroProximoId = 100;
                return nroProximoId;
            }

            nroProximoId = nroProximoId + 1;
            return nroProximoId;
        }
        //Verificar se já existe um registro para evitar duplicidades
        public bool existe(string descricao, int idCategoria, int id, int? idOrganizacaoInf = null) {
            if (idOrganizacao > 0 && idOrganizacaoInf == null) {
                idOrganizacaoInf = idOrganizacao;
            }

            var query = (from T in db.TipoAssociado where T.descricao == descricao && T.id != id && T.flagExcluido == "N" && T.idCategoria == idCategoria select T).AsNoTracking();


            if (idOrganizacaoInf > 0) {
                query = query.Where(x => x.idOrganizacao == idOrganizacaoInf);
            }

            if (idOrganizacaoInf == 0) {
                query = query.Where(x => x.idOrganizacao == null);
            }

            var OTipoTitulo = query.Take(1).FirstOrDefault();
            return (OTipoTitulo != null);
        }
        //Verificar se já existe um registro para evitar duplicidades
        public bool ehEstudante(int id) {
            var OTipo = this.carregar(id);

            if (OTipo == null) {
                return false;
            }

            return OTipo.flagEstudante;
        }

        /*Rotinas de Exclusão*/
        //Exclusão logica de registro
        public UtilRetorno excluir(int id) {

            var OAssociadoTipo = this.carregar(id);

            if (OAssociadoTipo == null) {
                return UtilRetorno.newInstance(true, "Não foi possível remover esse registro.");
            }
            var idUsuario = User.id();
            this.db.TipoAssociado
                .Where(x => x.id == id)
                .Update(x => new TipoAssociado{ flagExcluido = "S", idUsuarioAlteracao = idUsuario, dtAlteracao = DateTime.Now });

            return UtilRetorno.newInstance(false, "Registro removido com sucesso.");
        }

    }
}