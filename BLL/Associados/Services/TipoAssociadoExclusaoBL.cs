using System;
using System.Linq;
using System.Data.Entity;
using DAL.Associados;
using BLL.Services;
using DAL.Permissao.Security.Extensions;
using EntityFramework.Extensions;

namespace BLL.Associados {

	public class TipoAssociadoExclusaoBL : TipoAssociadoConsultaBL, ITipoAssociadoExclusaoBL {

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