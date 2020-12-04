using System;
using System.Linq;
using BLL.Services;
using DAL.Associados;

namespace BLL.Associados {

    public interface IAssociadoEmailVWBL {

        IQueryable<AssociadoEmailVW> listar(int? idTipoEmail, string flagSituacao, string valorBusca, string ativo);

    }
}