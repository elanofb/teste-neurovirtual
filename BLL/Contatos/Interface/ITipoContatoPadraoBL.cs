using DAL.Contatos;
using DAL.Repository.Base;
using System;
using System.Linq;
using BLL.Services;

namespace BLL.Contatos {

	public interface ITipoContatoPadraoBL {

        IQueryable<TipoContatoPadrao> listar(string ativo);

	}
}