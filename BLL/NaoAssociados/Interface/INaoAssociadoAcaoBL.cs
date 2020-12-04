using System;
using DAL.Associados;

namespace BLL.NaoAssociados {

    public interface INaoAssociadoAcaoBL {

        UtilRetorno desativarNaoAssociado(int idAssociado, string observacoes);

        UtilRetorno reativarNaoAssociado(int idAssociado, string observacoes);

        UtilRetorno reenviarSenha(int idAssociado);

        UtilRetorno enviarLinkSenha(int idAssociado);

        UtilRetorno excluirNaoAssociado(int idAssociado, string observacoes);

        UtilRetorno tornarAssociado(Associado Associado, string observacoes);

        UtilRetorno alterarTipo(int idAssociado, int idNovoTipo, int idUsuarioAlteracao);
    }
}