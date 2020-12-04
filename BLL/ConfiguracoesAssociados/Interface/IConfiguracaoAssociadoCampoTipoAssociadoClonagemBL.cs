using System;
using System.Collections.Generic;
using System.Linq;
using DAL.ConfiguracoesAssociados;

namespace BLL.ConfiguracoesAssociados {

    public interface IConfiguracaoAssociadoCampoTipoAssociadoClonagemBL {

        bool clonarConfiguracaoCampos(int idOrganizacaoInf, int idTipoAssociadoOrigem, List<int> listIdTipoAssociadoDestino);

    }
}