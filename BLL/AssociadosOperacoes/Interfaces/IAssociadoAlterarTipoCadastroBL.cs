using System;
using System.Collections.Generic;
using DAL.Associados.DTO;

namespace BLL.AssociadosOperacoes {

    public interface IAssociadoAlterarTipoCadastroBL {

        UtilRetorno alterarTipoCadastro(List<ItemListaAssociado> listaAssociados, byte? idTipoCadastro, int? idTipoAssociado);

    }

}
