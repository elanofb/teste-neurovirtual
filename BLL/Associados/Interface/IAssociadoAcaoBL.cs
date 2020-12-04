using System;
using System.Collections.Generic;
using DAL.AssociadosContribuicoes;

namespace BLL.Associados {

    public interface IAssociadoAcaoBL {

        UtilRetorno admitirAssociado(int idAssociado, DateTime? dtAdmissao);
        
        UtilRetorno bloquearAssociado(int idAssociado, string observacoes);
        
        UtilRetorno reenviarSenha(int idAssociado);

        UtilRetorno enviarLinkSenha(int idAssociado, string linkRecuperacao = "");

        UtilRetorno alterarTipo(int idAssociado, int idNovoTipo, int idUsuarioAlteracao);

        bool atualizarUltimoPagamentoContribuicao(AssociadoContribuicao OAssociadoContribuicao);
    }
}