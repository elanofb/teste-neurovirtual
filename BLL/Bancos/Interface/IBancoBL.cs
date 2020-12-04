using DAL.Bancos;
using System;
using System.Json;
using System.Linq;

namespace BLL.Bancos {

    public interface IBancoBL {

        Banco carregar(int id);

        IQueryable<Banco> listar(string valorBusca,string ativo);

        bool existe(string descricao, string nro, int id);

        bool salvar(Banco OTipoProduto);

        JsonMessageStatus alterarStatus(int id);

        UtilRetorno excluir(int id, int idUsuarioExclusao);

	}
}
