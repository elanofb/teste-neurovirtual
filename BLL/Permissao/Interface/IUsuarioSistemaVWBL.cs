using System;
using System.Linq;
using System.Collections.Generic;
using DAL.Permissao;

namespace BLL.Permissao {

	public interface IUsuarioSistemaVWBL {

		UsuarioSistemaVW carregar(int id);
        IQueryable<UsuarioSistemaVW> listar(int idPerfilAcesso, string valorBusca, string ativo);
		
	}
}
