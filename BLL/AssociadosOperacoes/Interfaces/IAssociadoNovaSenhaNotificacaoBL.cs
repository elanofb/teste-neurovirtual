using System;
using System.Collections.Generic;
using DAL.Associados.DTO;

namespace BLL.AssociadosOperacoes {

    public interface IAssociadoNovaSenhaNotificacaoBL {

        UtilRetorno registrarNovaSenha(List<ItemListaAssociado> listaAssociados, string senhaProvisoria);

    }

}
