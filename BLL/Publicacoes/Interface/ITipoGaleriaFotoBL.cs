using System;
using System.Json;
using System.Linq;
using DAL.Publicacoes;

namespace BLL.Publicacoes {

    public interface ITipoGaleriaFotoBL {

        TipoGaleriaFoto carregar(int id);

        IQueryable<TipoGaleriaFoto> listar(string valorBusca, bool? ativo, bool? flagGaleriaAtiva = null);

        bool salvar(TipoGaleriaFoto OTipoGaleriaFoto);

        bool existe(string descricao, int id);

        bool excluir(int id);

        JsonMessageStatus alterarStatus(int id);
    }
}
