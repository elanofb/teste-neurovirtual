using System;
using System.Linq;
using System.Data.Entity;
using DAL.Associados;
using BLL.Services;
using DAL.Permissao.Security.Extensions;
using EntityFramework.Extensions;

namespace BLL.Associados {
	public class CategoriaTipoAssociadoExclusaoBL : CategoriaTipoAssociadoConsultaBL, ICategoriaTipoAssociadoExclusaoBL {
		//Exclusao logica de registro
		public UtilRetorno excluir(int id) {

			var OCategoriaTipo = this.carregar(id);

			if (OCategoriaTipo == null) {
				return UtilRetorno.newInstance(true, "Não foi possível remover esse registro.");
			}

			var idUsuario = User.id();

			this.db.CategoriaTipoAssociado.Where(x => x.id == id)
				.Update(x => new CategoriaTipoAssociado{ flagExcluido = "S", idUsuarioAlteracao = idUsuario, dtAlteracao = DateTime.Now });

			return UtilRetorno.newInstance(false, "Registro removido com sucesso.");
		}
	}
}