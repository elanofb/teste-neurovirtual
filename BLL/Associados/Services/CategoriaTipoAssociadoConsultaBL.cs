using System;
using System.Linq;
using System.Data.Entity;
using DAL.Associados;
using BLL.Services;
using DAL.Permissao.Security.Extensions;
using EntityFramework.Extensions;

namespace BLL.Associados {
	public class CategoriaTipoAssociadoConsultaBL : DefaultBL, ICategoriaTipoAssociadoConsultaBL {
		//Carregamento de registro pelo ID
		public CategoriaTipoAssociado carregar(int id, int? idOrganizacaoInf = null) {

			if (idOrganizacao > 0 && idOrganizacaoInf == null) {
				idOrganizacaoInf = idOrganizacao;
			}

			var query = (from CT in db.CategoriaTipoAssociado
				where
					CT.flagExcluido == "N" &&
					CT.id == id
				select CT);

			if (idOrganizacaoInf > 0) {
				query = query.Where(x => x.idOrganizacao == idOrganizacaoInf);
			}

			if (idOrganizacaoInf == 0) {
				query = query.Where(x => x.idOrganizacao == null);
			}

			return query.FirstOrDefault();
		}
		//listar registros do banco com base nos parametros
		public IQueryable<CategoriaTipoAssociado> listar(string valorBusca, string ativo) {

			var query = from CT in db.CategoriaTipoAssociado
				where CT.flagExcluido == "N"
				select CT;

			query = query.condicoesSeguranca();

			if (!String.IsNullOrEmpty(valorBusca)) {
				query = query.Where(x => x.descricao.Contains(valorBusca));
			}

			if (!String.IsNullOrEmpty(ativo)) {
				query = query.Where(x => x.ativo == ativo);
			}

			return query;
		}
	}
}