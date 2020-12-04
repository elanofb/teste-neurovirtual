using System;
using System.Collections.Generic;
using System.Linq;
using BLL.Services;
using DAL.Associados;

namespace BLL.Associados {

    public interface IAssociadoAreaAtuacaoVWBL {

        IQueryable<AssociadoAreaAtuacaoVW> listar(List<int> idsAreaAtuacao, string flagSituacao, string valorBusca, string ativo);

    }
}