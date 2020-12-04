using System;
using System.Collections.Generic;
using System.Linq;
using DAL.Associados;
using DAL.Pessoas;

namespace BLL.NaoAssociadosInstitucional {

	public interface INaoAssociadoAcessoBL {

		Associado login(string login, string senha, int? idOrganizacaoParam = null);
	}
}
