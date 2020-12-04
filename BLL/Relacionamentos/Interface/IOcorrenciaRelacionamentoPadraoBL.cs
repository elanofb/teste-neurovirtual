using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Relacionamentos;

namespace BLL.Relacionamentos {
    public interface IOcorrenciaRelacionamentoPadraoBL {

        OcorrenciaRelacionamentoPadrao carregar(int id);
        IQueryable<OcorrenciaRelacionamentoPadrao> listar(string valorBusca, string ativo, string flagSistema = "N");
        bool existe(string descricao, int id);
        bool salvar(OcorrenciaRelacionamentoPadrao OOcorrenciaRelacionamento);
        bool excluir(int id);

    }
}
