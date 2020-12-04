using System;
using System.Linq;
using System.Data.Entity;
using DAL.Associados;
using BLL.Services;
using DAL.Permissao.Security.Extensions;
using EntityFramework.Extensions;

namespace BLL.Associados {

	public class TipoAssociadoConsultaBL : DefaultBL, ITipoAssociadoConsultaBL {

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
	}
}