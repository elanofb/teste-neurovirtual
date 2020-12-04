using System;
using System.Linq;
using System.Data.Entity;
using DAL.Funcionarios;
using DAL.Pessoas;
using DAL.Documentos;
using DAL.Enderecos;
using DAL.Repository.Base;
using System.Data.Entity.Core.Objects;
using EntityFramework.Extensions;
using System.Json;
using BLL.Services;
using DAL.Permissao.Security.Extensions;
using UTIL.Resources;

namespace BLL.Funcionarios {

	public class FuncionarioExclusaoBL : DefaultBL, IFuncionarioExclusaoBL {

		//
		public FuncionarioExclusaoBL() {
		}

		//Excluir uma Funcionario logicamente
		public bool excluir(int id) {
			int idUsuarioLogado = User.id();

			db.Funcionario
				.Where(x => x.id == id)
				.Update(x => new Funcionario { flagExcluido = "S", dtAlteracao = DateTime.Now, idUsuarioAlteracao = idUsuarioLogado });

			return true;
		}

	}
}