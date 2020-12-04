using System.Linq;
using DAL.Localizacao;
using System;
using System.Json;

namespace BLL.Localizacao {

	public interface IPaisBL {
        
        Pais carregar(string id);

        IQueryable<Pais> listar(string valorBusca, string ativo);

        bool inserir(Pais OPais);

        bool atualizar(Pais OPais);

        JsonMessageStatus alterarStatus(string id);

        UtilRetorno excluir(string id, int idUsuarioExclusao);

	}
}
