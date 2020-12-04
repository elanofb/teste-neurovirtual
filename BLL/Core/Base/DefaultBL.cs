using System;
using System.Security.Principal;
using DAL.Permissao.Security.Extensions;
using DAL.Repository.Base;

namespace BLL.Services {

	public class DefaultBL {
		//Atributos
		private DataContext _DataContext;
		//Propriedades
		public DataContext db => this._DataContext = this._DataContext ?? new DataContext();
	    protected IPrincipal User => HttpContextFactory.Current.User;
	    protected int idOrganizacao => User.idOrganizacao();
	}
}