using DAL.Localizacao;
using System;
using System.Json;
using System.Linq;

namespace BLL.Localizacao {

    public interface IEstadoBL {
                        
        Estado carregar(string sigla);

        Estado carregarPorId(int id);

        IQueryable<Estado> listar(string valorBusca, string ativo);

        bool salvar(Estado OEstado);

        JsonMessageStatus alterarStatus(int id);

        UtilRetorno excluir(int id);

	}
}
