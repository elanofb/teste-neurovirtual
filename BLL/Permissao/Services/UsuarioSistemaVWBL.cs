using System;
using System.Collections.Generic;
using System.Json;
using System.Linq;
using BLL.Email;
using DAL.Permissao;
using DAL.Repository.Base;
using EntityFramework.Extensions;
using BLL.Services;
using System.Data.Entity;

namespace BLL.Permissao {

	public class UsuarioSistemaVWBL : DefaultBL, IUsuarioSistemaVWBL {

		public UsuarioSistemaVWBL() {
			
		}

        public UsuarioSistemaVW carregar(int id) {

			var query = from Usuario in db.UsuarioSistemaVW
						where Usuario.id == id
						select Usuario;

			return query.FirstOrDefault();
		}

		//
		public IQueryable<UsuarioSistemaVW> listar(int idPerfilAcesso, string valorBusca, string ativo) {

			var query = from Usuario in db.UsuarioSistemaVW
						select Usuario;

			if (idPerfilAcesso > 0) {
				query = query.Where(x => x.idPerfilAcesso == idPerfilAcesso);
			}

			if (!String.IsNullOrEmpty(valorBusca)) {
				query = query.Where(x => x.nome.Contains(valorBusca));
			}

			if (!String.IsNullOrEmpty(ativo)) {
				query = query.Where(x => x.ativo == ativo);
			}

			return query;
		}

	}
}